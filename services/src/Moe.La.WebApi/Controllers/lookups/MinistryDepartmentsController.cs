using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/ministry-departments")]
    [Authorize]

    public class MinistryDepartmentsController : ControllerBase
    {
        private readonly IMinistryDepartmentService _ministryDepartmentService;

        public MinistryDepartmentsController(IMinistryDepartmentService ministryDepartmentService)
        {
            _ministryDepartmentService = ministryDepartmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(MinistryDepartmentQueryObject queryObject)
        {
            var result = await _ministryDepartmentService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _ministryDepartmentService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MinistryDepartmentDto ministryDepartmentDto)
        {

            var result = await _ministryDepartmentService.AddAsync(ministryDepartmentDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MinistryDepartmentDto ministryDepartmentDto)
        {

            var result = await _ministryDepartmentService.EditAsync(ministryDepartmentDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = Guid.Parse(User.FindFirst("id").Value);

            var result = await _ministryDepartmentService.RemoveAsync(id, userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("is-name-exists")]
        public async Task<IActionResult> IsNameExists([FromBody] MinistryDepartmentDto ministryDepartmentDto)
        {
            var result = await _ministryDepartmentService.IsNameExistsAsync(ministryDepartmentDto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
