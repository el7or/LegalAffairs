using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moe.La.Core.Entities;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/app-settings")]
    public class AppSettingsController : ControllerBase
    {
        private readonly AppSettings appSettings;

        public AppSettingsController(IOptionsSnapshot<AppSettings> options)
        {
            appSettings = options.Value;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                appSettings.SystemName,
                appSettings.AdminEmail,
                appSettings.IsLawFirmOffice,
                appSettings.AgainstCasesRequiresRating
            });
        }
    }
}