using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moe.La.Core.Entities;
using Moe.La.Core.Integration;
using Moe.La.Integration;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/email")]
    [Authorize]

    public class EmailController : ControllerBase
    {
        private readonly IEmailNotification emailNotification;

        public EmailController(IOptionsSnapshot<AppSettings> options,
           IHttpContextAccessor httpContextAccessor)
        {
            emailNotification = new EmailNotification(
                options.Value, httpContextAccessor); // AppSettings
        }

        [HttpPost]
        public IActionResult SendEmail(string to, string subject, string message)
        {
            var result = emailNotification.Send(to, subject, emailNotification.GetTemplate(message));

            return Ok(new { result });
        }
    }
}