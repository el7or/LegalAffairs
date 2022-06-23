using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/investigation-records")]
    [Authorize]

    public class InvestigationRecordsController : ControllerBase
    {
        private readonly IInvestigationRecordService _investigationRecordService;

        public InvestigationRecordsController(IInvestigationRecordService investigationRecordService)
        {
            _investigationRecordService = investigationRecordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(InvestiationRecordQueryObject queryObject)
        {
            var result = await _investigationRecordService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _investigationRecordService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InvestigationRecordDto investigationRecordDto)
        {
            var result = await _investigationRecordService.AddAsync(investigationRecordDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InvestigationRecordDto investigationRecordDto)
        {
            var result = await _investigationRecordService.EditAsync(investigationRecordDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _investigationRecordService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
