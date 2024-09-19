using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmallShop.Services.Users.Contracts;
using SmallShop.Services.Users.Contracts.Dtos;

namespace SmallShop.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<int> Add(AddUserDto dto)
        {
            return await _userService.Add(dto);
        }

        [HttpPost("login")]
        public async Task<TokenResponseDto> Login([FromBody] LoginDto dto)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress!.ToString();
            return await _userService.LoginAsync(dto,ipAddress);
        }

        [HttpPost("refresh-token")]
        public async Task<TokenResponseDto> GetRefreshToken(string refreshToken)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress!.ToString();
            return await _userService.RefreshTokenAsync(refreshToken, ipAddress);
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IActionResult> GetNames()
        {
            var names = await
                Task.FromResult(new List<string>() { "Mojtaba", "Khoshnam" });
            return Ok(names);
        }
    }
}
