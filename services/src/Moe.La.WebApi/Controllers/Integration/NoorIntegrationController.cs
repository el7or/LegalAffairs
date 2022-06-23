using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers.Integration
{
    [Route("api/integration/noor")]
    [AllowAnonymous]
    public class NoorIntegrationController : ControllerBase
    {
        private readonly INoorIntegrationService _noorIntegrationService;

        public NoorIntegrationController(INoorIntegrationService noorIntegrationService)
        {
            _noorIntegrationService = noorIntegrationService;
        }

        [HttpGet("find-party/{searchText}")]
        public async Task<IActionResult> FindUserAsync(string searchText, int? investigationRecordId)
        {
            var result = await _noorIntegrationService.GetAsync(searchText, investigationRecordId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
