using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.IntegrationControllers
{
    [Route("api/integration/najiz-case-log")]
    [AllowAnonymous]
    public class NajizCaseLogController : ControllerBase
    {
        private readonly INajizCaseLogService _najizCaseLogService;

        public NajizCaseLogController(INajizCaseLogService najizCaseLogService)
        {
            _najizCaseLogService = najizCaseLogService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] NajizCaseDto najizCase)
        {
            var result = await _najizCaseLogService.AddAsync(najizCase);
            return StatusCode((int)result.StatusCode, result);
        }


    }
}
