using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/branches")]
    [Authorize]

    public class BranchController : ControllerBase
    {
        private readonly IBranchService _BranchService;

        public BranchController(IBranchService BranchService)
        {
            _BranchService = BranchService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(BranchQueryObject queryObject)
        {
            dynamic result;

            if (queryObject == null || queryObject.Page == default)
            {
                result = await _BranchService.GetAllAsync();
            }
            else
            {
                result = await _BranchService.GetAllAsync(queryObject);
            }

            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _BranchService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}/departments")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var result = await _BranchService.GetDepartmentsAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> CheckNameExists(string name)
        {
            var result = await _BranchService.GetByNameAsync(name);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BranchDto generalManagmentDto)
        {
            var result = await _BranchService.AddAsync(generalManagmentDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BranchDto generalManagmentDto)
        {
            var result = await _BranchService.EditAsync(generalManagmentDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _BranchService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("is-name-exists")]
        public async Task<IActionResult> IsNameExists([FromBody]
        BranchDto generalManagmentDto)
        {
            var result = await _BranchService.IsNameExistsAsync(generalManagmentDto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}