using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moe.La.Core.Entities;
using Moe.La.Integration;
using System.IO;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/error")]
    public class ErrorController : ControllerBase
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly EmailNotification emailNotification;

        public ErrorController(IOptionsSnapshot<AppSettings> options, IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
            emailNotification = new EmailNotification(options.Value); // AppSettings
        }

        [HttpPost("logging")]
        public IActionResult Logging(string errorMessage)
        {
            string webRootPath = hostingEnvironment.WebRootPath;
            string path = Path.Combine(webRootPath, "ErrorsLog.txt");
            return Ok(Errors.Log(path, errorMessage));
        }

        [HttpPost("send-email")]
        public IActionResult SendEmail(string errorMessage)
        {
            var result = emailNotification.Send("nashwan.nasser@gmail.com", "AppCore Error !!!", errorMessage);
            return Ok(new { result });
        }
    }
}