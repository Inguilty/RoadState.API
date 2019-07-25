using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadState.BusinessLayer.Shared.Helpers;
using RoadState.BusinessLayer.Shared.Interfaces;
using RoadState.BusinessLayer.Shared.TransportModels;
using RoadState.Data;
using RoadState.DataAccessLayer;

namespace RoadState.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly RoadStateContext _context;
        private readonly IMapper _mapper;

        public UserService(RoadStateContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserTransportModel> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            var authenticatedUser = _mapper.Map<UserTransportModel>(user);

            return authenticatedUser;
        }

        public async Task<UserTransportModel> GetById(string id)
        {
            var user = await _context.Users.FindAsync(id);
            var foundUser = _mapper.Map<UserTransportModel>(user);
            return foundUser;
        }

        public async Task<UserTransportModel> Create(UserTransportModel user, string password)
        {
            var createdUser = _mapper.Map<User>(user);
            
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (await _context.Users.AnyAsync(x => x.UserName == createdUser.UserName))
                throw new AppException($"Username {createdUser.UserName} is already taken");

            if (await _context.Users.AnyAsync(x => x.Email == createdUser.Email))
                throw new AppException($"Email {createdUser.Email} is already taken");

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            createdUser.PasswordHash = passwordHash;
            createdUser.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(createdUser);
            await _context.SaveChangesAsync();
 
            return user;
        }

        public async void Update(UserTransportModel userParam, string password = null)
        {
            var user = await _context.Users.FindAsync(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.UserName != user.UserName)
            {
                // username has changed so check if the new username is already taken
                if (_context.Users.Any(x => x.UserName == userParam.UserName))
                    throw new AppException($"Username {userParam.UserName} is already taken");
                if (_context.Users.Any(x => x.Email == userParam.Email))
                    throw new AppException($"Email {userParam.Email} is already taken");
            }

            // update user properties
            user.UserName = userParam.UserName;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
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
