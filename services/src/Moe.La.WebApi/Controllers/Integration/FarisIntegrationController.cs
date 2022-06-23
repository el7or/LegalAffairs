using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/integration/faris")]
    [AllowAnonymous]
    public class FarisIntegrationController : ControllerBase
    {
        private readonly IFarisIntegrationService _farisIntegrationService;

        public FarisIntegrationController(IFarisIntegrationService farisIntegrationService)
        {
            _farisIntegrationService = farisIntegrationService;
        }

        [HttpGet("find-party/{searchText}")]
        public async Task<IActionResult> FindUserAsync(string searchText, int? investigationRecordId)
        {
            var result = await _farisIntegrationService.GetAsync(searchText, investigationRecordId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("user/{id}")]
        public IActionResult GetUserAsync(string id)
        {
            var result = _farisIntegrationService.GetUserAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
