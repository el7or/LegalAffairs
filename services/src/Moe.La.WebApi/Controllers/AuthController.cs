using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [HttpPost("login/{rule_name}")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] CredentialsDto credentials, string rule_name = null)
        {
            var result = await _authService.Login(credentials, rule_name = null);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            var result = await _authService.RefreshToken(refreshToken);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken(string token)
        {
            var result = await _authService.RevokeToken(token);
            return StatusCode((int)result.StatusCode, result);
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refresh_token", token, cookieOptions);
        }
    }
}
