using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RoadState.Backend.Helpers;
using RoadState.Backend.Models;
using RoadState.BusinessLayer.Shared.Helpers;
using RoadState.BusinessLayer.Shared.Interfaces;
using RoadState.BusinessLayer.Shared.TransportModels;

namespace RoadState.Backend.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AuthorizationController(
            IUserService userService,
            IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]UserModel userModelDto)
        {
            var user = await _userService.Authenticate(userModelDto.UserName, userModelDto.Password, _appSettings.Secret);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(new
            {
                id = user.Id,
                token = user.Token
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserModel userModelDto)
        {
            var user = _mapper.Map<UserTransportModel>(userModelDto);

            try
            {
                await _userService.Create(user, user.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/update")]
        public IActionResult Update([FromBody]UserModel userModelDto)
        {
            var user = _mapper.Map<UserTransportModel>(userModelDto);

            try
            {
                var updated = _userService.Update(user, userModelDto.NewPassword);
                if(!updated.Result)
                    throw new AppException("Your old password is wrong!");
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
