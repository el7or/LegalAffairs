using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.Job.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHangfireAuthService _authService;

        public AccountController(IHangfireAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("token") != null)
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(HttpContext.Session.GetString("token"));

                if (token != null && token.Claims.Any(n => n.Type == "roles" && n.Value == ApplicationRolesConstants.Admin.Name))
                {
                    return Redirect("/hangfire");
                }
                else if (token != null)
                {
                    HttpContext.Session.Remove("token");

                    return RedirectToAction("Login");
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(CredentialsDto model)
        {

            var result = await _authService.Login(model);

            if (result.IsSuccess && result.ErrorList == null)
            {

                HttpContext.Session.SetString("token", result.Data);

                return Redirect("/hangfire");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");

            return RedirectToAction("Login");
        }
    }
}
