using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/sub-work-item-type")]
    [Authorize]
    public class SubWorkItemTypeController : ControllerBase
    {
        private readonly ISubWorkItemTypeService _subWorkItemTypeService;

        public SubWorkItemTypeController(ISubWorkItemTypeService subWorkItemTypeService)
        {
            _subWorkItemTypeService = subWorkItemTypeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(SubWorkItemTypeQueryObject queryObject)
        {
            var result = await _subWorkItemTypeService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _subWorkItemTypeService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubWorkItemTypeDto subWorkItemTypeDto)
        {

            var result = await _subWorkItemTypeService.AddAsync(subWorkItemTypeDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SubWorkItemTypeDto subWorkItemType)
        {
            var result = await _subWorkItemTypeService.EditAsync(subWorkItemType);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _subWorkItemTypeService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("is-name-exists")]
        public async Task<IActionResult> IsNameExists([FromBody] SubWorkItemTypeDto subWorkItemType)
        {
            var result = await _subWorkItemTypeService.IsNameExistsAsync(subWorkItemType);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
