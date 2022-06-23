using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models.Integration.Moeen;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers.Integration
{
    [Route("api/integration/moeen")]
    [ApiController]
    public class MoeenController : ControllerBase
    {
        private readonly IMoeenInformLetterIntegrationService _moeenInformLetterIntegrationService;

        public MoeenController(IMoeenInformLetterIntegrationService moeenInformLetterIntegrationService)
        {
            _moeenInformLetterIntegrationService = moeenInformLetterIntegrationService;
        }

        [HttpPost]
        [Route("inform-letter")]
        public async Task<IActionResult> AddInformLetterAsync([FromBody] InformLetterInfoStructureModel informLetter)
        {
            var result = await _moeenInformLetterIntegrationService.AddAsync(informLetter);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
