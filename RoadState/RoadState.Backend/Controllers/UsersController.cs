using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RoadState.Backend.Helpers;
using RoadState.Backend.Models;
using RoadState.BusinessLayer.Services;
using RoadState.BusinessLayer.TransportModels;
using RoadState.DataAccessLayer;
using System.Linq;

namespace RoadState.Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IUserFinder userFinder;

        public AuthorizationController(
            IUserService userService,
            IMapper mapper, IOptions<AppSettings> appSettings,
            IUserFinder userFinder)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            this.userFinder = userFinder;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserProfile user)
        {
            var result = await _userService.Authenticate(user.UserName, user.Password, _appSettings.Secret);

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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserInfo(string userId)
        {
            if (userId is null) return BadRequest("Invalid input");
            var result = await userFinder.GetUsersAsync(x => x.Id == userId);
            if (result.Count == 0) return NotFound("No such user");
            return Ok(_mapper.Map<UserDto>(result.FirstOrDefault()));
        }
        [Authorize]
        [HttpGet("checkToken")]
        public async Task<IActionResult> CheckToken()
        {
            if (!await CheckTokenValid()) return BadRequest();
            var tokenString = Request.Headers["Authorization"].ToString().Split(' ')[1];
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return Ok(new
            {
                id = userId,
                token = tokenString
            });
        }


        private async Task<bool> CheckTokenValid()
        {
            var tokenString = Request.Headers["Authorization"].ToString().Split(' ')[1];
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(tokenString) as JwtSecurityToken;

            //If there is no user with such id return false
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
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
    }
}
