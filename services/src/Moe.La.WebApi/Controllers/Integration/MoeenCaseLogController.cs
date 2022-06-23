using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.IntegrationControllers
{
    [Route("api/integration/moeen-case-log")]
    [AllowAnonymous]
    public class MoeenCaseLogController : ControllerBase
    {
        private readonly IMoeenCaseLogService _moeenCaseLogService;

        public MoeenCaseLogController(IMoeenCaseLogService moeenCaseLogService)
        {
            _moeenCaseLogService = moeenCaseLogService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] MoeenCaseDto moeenCase)
        {
            var result = await _moeenCaseLogService.AddAsync(moeenCase);
            return StatusCode((int)result.StatusCode, result);
        }


    }
}
