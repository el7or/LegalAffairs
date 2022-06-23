using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/government-organizations")]
    [Authorize]

    public class GovernmentOrganizationsController : ControllerBase
    {
        private readonly IGovernmentOrganizationService _governmentOrganizationService;

        public GovernmentOrganizationsController(IGovernmentOrganizationService governmentOrganizationService)
        {
            _governmentOrganizationService = governmentOrganizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(QueryObject queryObject)
        {
            var result = await _governmentOrganizationService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _governmentOrganizationService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GovernmentOrganizationDto governmentOrganizationDto)
        {

            var result = await _governmentOrganizationService.AddAsync(governmentOrganizationDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] GovernmentOrganizationDto governmentOrganizationDto)
        {

            var result = await _governmentOrganizationService.EditAsync(governmentOrganizationDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _governmentOrganizationService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("is-name-exists/{name}")]
        public async Task<IActionResult> IsNameExists(string name)
        {
            var result = await _governmentOrganizationService.IsNameExistsAsync(name);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
