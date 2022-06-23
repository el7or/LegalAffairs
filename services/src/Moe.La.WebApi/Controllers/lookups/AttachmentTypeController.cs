using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/attachment-types")]
    [Authorize]

    public class AttachmentTypesController : ControllerBase
    {
        private readonly IAttachmentTypeService _attachmentTypeService;

        public AttachmentTypesController(IAttachmentTypeService attachmentTypeService)
        {
            _attachmentTypeService = attachmentTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(AttachmentQueryObject queryObject)
        {
            var result = await _attachmentTypeService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _attachmentTypeService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AttachmentTypeDto attachmentTypeDto)
        {
            var result = await _attachmentTypeService.AddAsync(attachmentTypeDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AttachmentTypeDto attachmentTypeDto)
        {
            var result = await _attachmentTypeService.EditAsync(attachmentTypeDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _attachmentTypeService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("group-names")]
        public IActionResult GetHearingStatus()
        {
            return Ok(EnumExtensions.GetValues<GroupNames>());
        }

        [HttpPost("is-name-exists")]
        public async Task<IActionResult> IsNameExists([FromBody] AttachmentTypeDto attachmentTypeDto)
        {
            var result = await _attachmentTypeService.IsNameExistsAsync(attachmentTypeDto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
