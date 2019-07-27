using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RoadState.BusinessLayer.Helpers;
using RoadState.BusinessLayer.TransportModels;
using RoadState.Data;
using RoadState.DataAccessLayer;

namespace RoadState.BusinessLayer.Services
{
    public interface IUserService
    {
        Task<UserAuthenticateResult> Authenticate(string username, string password, string appSettings);
        Task<UserDto> GetById(string id);
        Task<UserAuthenticateResult> Create(UserDto user, string password);
        Task<UserUpdateResult> Update(UserDto user, string newPassword);
    }

    public class UserService : IUserService
    {
        private readonly IUserUpdator _userUpdater;
        private readonly IUserFinder _userFinder;
        private readonly IUserCreator _userCreator;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, IUserUpdator userUpdater, IUserFinder userFinder, IUserCreator userCreator)
        {
            _userCreator = userCreator;
            _userFinder = userFinder;
            _userUpdater = userUpdater;
            _mapper = mapper;
        }

        public async Task<UserAuthenticateResult> Authenticate(string username, string password, string appSettings)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    throw new WrongCredentialsException("Wrong username or password!");

                var result = await _userFinder.GetUsersAsync(x => x.UserName == username);
                var user = result.FirstOrDefault();
                if (user == null)
                    throw new UserNotFoundException("User not found!");

                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                    throw new WrongCredentialsException("You entered wrong old password!");

                var authenticatedUser = _mapper.Map<UserDto>(user);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(appSettings);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                authenticatedUser.Token = tokenString;

                return new UserAuthenticateResult
                {
                    User = authenticatedUser
                };
            }
            catch (UserNotFoundException e)
            {
                return new UserAuthenticateResult { ErrorOccured = true, ErrorMessage = e.Message };
            }
            catch (WrongCredentialsException e)
            {
                return new UserAuthenticateResult { ErrorOccured = true, ErrorMessage = e.Message };
            }
            catch (Exception e)
            {
                return new UserAuthenticateResult { ErrorOccured = true, ErrorMessage = e.Message };
            }
        }

        public async Task<UserDto> GetById(string id)
        {
            var result = await _userFinder.GetUsersAsync(x => x.Id == id);
            var user = result.FirstOrDefault();
            if (user == null)
                throw new UserNotFoundException("User not found!");
            var foundUser = _mapper.Map<UserDto>(user);
            return foundUser;
        }

        public async Task<UserAuthenticateResult> Create(UserDto user, string password)
        {
            try
            {
                var createdUser = _mapper.Map<User>(user);

                if (string.IsNullOrWhiteSpace(password))
                    throw new WrongCredentialsException("Password is required");

                var checkUserName = await _userFinder.GetUsersAsync(x => x.UserName == createdUser.UserName);
                if (checkUserName.FirstOrDefault() == null)
                    throw new WrongCredentialsException($"Username {createdUser.UserName} is already taken");

                var checkEmail = await _userFinder.GetUsersAsync(x => x.Email == createdUser.Email);

                if (checkEmail.FirstOrDefault() == null)
                    throw new WrongCredentialsException($"Email {createdUser.Email} is already taken");

                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                createdUser.PasswordHash = passwordHash;
                createdUser.PasswordSalt = passwordSalt;

                await _userCreator.CreateUserAsync(createdUser);

                return new UserAuthenticateResult
                {
                    User = user
                };
            }
            catch (WrongCredentialsException e)
            {
                return new UserAuthenticateResult { ErrorOccured = true, ErrorMessage = e.Message };
            }
            catch (Exception e)
            {
                return new UserAuthenticateResult { ErrorOccured = true, ErrorMessage = e.Message };
            }
        }

        public async Task<UserUpdateResult> Update(UserDto userParam, string newPassword)
        {
            try
            {
                var result = await _userFinder.GetUsersAsync(x => x.Id == userParam.Id);
                var user = result.FirstOrDefault();

                if (user == null)
                    throw new UserNotFoundException("User not found!");

                if (!string.IsNullOrEmpty(userParam.AvatarUrl))
                {
                    user.AvatarUrl = userParam.AvatarUrl;
                }

                if (!VerifyPasswordHash(userParam.Password, user.PasswordHash, user.PasswordSalt))
                    throw new WrongCredentialsException("You entered wrong old password!");

                // update password if it was entered
                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    CreatePasswordHash(userParam.Password, out byte[] passwordHash, out byte[] passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                }

                await _userUpdater.UpdateUserAsync(user);
                return new UserUpdateResult();
            }
            catch (UserNotFoundException e)
            {
                return new UserUpdateResult {ErrorOccured = true, ErrorMessage = e.Message};
            }
            catch (WrongCredentialsException e)
            {
                return new UserUpdateResult { ErrorOccured = true, ErrorMessage = e.Message };
            }
            catch (Exception e)
            {
                return new UserUpdateResult { ErrorOccured = true, ErrorMessage = e.Message };
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, IReadOnlyList<byte> storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            if (storedHash.Count != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
