using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/work-item-type")]
    [Authorize]
    public class WorkItemTypeController : ControllerBase
    {
        private readonly IWorkItemTypeService _workItemTypeService;

        public WorkItemTypeController(IWorkItemTypeService workItemTypeService)
        {
            _workItemTypeService = workItemTypeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(WorkItemTypeQueryObject queryObject)
        {
            var result = await _workItemTypeService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _workItemTypeService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] WorkItemTypeDto workItemTypeDto)
        {
            var result = await _workItemTypeService.EditAsync(workItemTypeDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _workItemTypeService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("is-name-exists")]
        public async Task<IActionResult> IsNameExists([FromBody] WorkItemTypeDto workItemTypeDto)
        {
            var result = await _workItemTypeService.IsNameExistsAsync(workItemTypeDto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
