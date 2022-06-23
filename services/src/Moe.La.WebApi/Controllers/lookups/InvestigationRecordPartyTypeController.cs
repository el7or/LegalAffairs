using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/investigation-record-party-type")]
    [Authorize]
    public class InvestigationRecordPartyTypeController : ControllerBase
    {
        private readonly IInvestigationRecordPartyTypeService _investigationRecordPartyTypeService;

        public InvestigationRecordPartyTypeController(IInvestigationRecordPartyTypeService investigationRecordPartyTypeService)
        {
            _investigationRecordPartyTypeService = investigationRecordPartyTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(QueryObject queryObject)
        {
            var result = await _investigationRecordPartyTypeService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _investigationRecordPartyTypeService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InvestigationRecordPartyTypeDto investigationRecordPartyTypeDto)
        {
            var result = await _investigationRecordPartyTypeService.AddAsync(investigationRecordPartyTypeDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InvestigationRecordPartyTypeDto investigationRecordPartyTypeDto)
        {
            var result = await _investigationRecordPartyTypeService.EditAsync(investigationRecordPartyTypeDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _investigationRecordPartyTypeService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("is-name-exists/{name}")]
        public async Task<IActionResult> IsNameExists(string name)
        {
            var result = await _investigationRecordPartyTypeService.IsNameExistsAsync(name);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
