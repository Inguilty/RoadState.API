using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RoadState.Backend.Models;
using RoadState.BusinessLayer.Services;
using RoadState.BusinessLayer.TransportModels;
using RoadState.DataAccessLayer;

namespace RoadState.Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUserFinder _userFinder;

        public AuthorizationController(
            IUserService userService,
            IMapper mapper, IConfiguration configuration,
            IUserFinder userFinder)
        {
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
            this._userFinder = userFinder;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserProfile user)
        {
            var key = _configuration["AppSettings:Secret"];
            var result = await _userService.Authenticate(user.UserName, user.Password, key);

            if (result.ErrorOccured)
                return BadRequest(new { message = result.ErrorMessage });


            return Ok(new
            {
                id = result.User.Id,
                token = result.User.Token
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserProfile userProfileDto)
        {
            var user = _mapper.Map<UserDto>(userProfileDto);
            var result = await _userService.Create(user, user.Password);

            if (result.ErrorOccured)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok();
        }

        [Authorize]
        [HttpPut("{id}/update")]
        public async Task<IActionResult> Update([FromBody]UserProfile userProfileDto)
        {
            if (!await CheckTokenValid()) return NoContent();
            var user = _mapper.Map<UserDto>(userProfileDto);
            var result = await _userService.Update(user, userProfileDto.NewPassword);

            if (result.ErrorOccured)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok();
        }


        [HttpGet("getUserCredentials")]
        public async Task<IActionResult> GetUserCredentials()
        {
            string userId = "";
            try
            {
                userId = GetIdFromClaims();
            }
            catch (Exception)
            {
                userId = "fe640abd-37af-4aa7-8b65-04d060200361";
            }
            if (userId is null)
            {
                userId = "fe640abd-37af-4aa7-8b65-04d060200361";
            }
            var foundUser = await _userFinder.GetUsersAsync(x => x.Id == userId);
            var result = _mapper.Map<UserDto>(foundUser?.FirstOrDefault());
            return Ok(new
            {
                userName = result.UserName,
                email = result.Email,
            });
        }

        [Authorize]
        [HttpGet("checkToken")]
        public async Task<IActionResult> CheckToken()
        {
            if (!await CheckTokenValid()) return BadRequest("You should Sign in again!");
            var tokenString = GetTokenFromRequest();
            var userId = GetIdFromClaims();
            return Ok(new
            {
                id = userId,
                token = tokenString
            });
        }

        private async Task<bool> CheckTokenValid()
        {
            var tokenString = GetTokenFromRequest();
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(tokenString) as JwtSecurityToken;

            //If there is no user with such id return false
            var userId = GetIdFromClaims();
            if (await _userService.GetById(userId) == null) return false;

            if (token == null) return false;
            var tokenExpiryDate = token.ValidTo;
            // If there is no valid `exp` claim then `ValidTo` returns DateTime.MinValue
            if (tokenExpiryDate == DateTime.MinValue) return false;

            // If the token is in the past then you can't use it
            if (tokenExpiryDate < DateTime.UtcNow) return false;

            // Token is valid
            return true;
        }
        private string GetIdFromClaims()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }

        private string GetTokenFromRequest()
        {
            var tokenString = Request.Headers["Authorization"].ToString().Split(' ')[1];
            return tokenString;
        }
    }
}
