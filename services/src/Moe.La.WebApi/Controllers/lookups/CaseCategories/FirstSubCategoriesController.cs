using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/first-sub-categories")]
    [Authorize]
    public class FirstSubCategoriesController : ControllerBase
    {
        private readonly IFirstSubCategoryService _firstSubCategoryService;

        public FirstSubCategoriesController(IFirstSubCategoryService firstSubCategoryService)
        {
            _firstSubCategoryService = firstSubCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(FirstSubCategoriesQueryObject queryObject)
        {
            var result = await _firstSubCategoryService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _firstSubCategoryService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FirstSubCategoryDto firstSubCategoryDto)
        {
            var result = await _firstSubCategoryService.AddAsync(firstSubCategoryDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] FirstSubCategoryDto firstSubCategoryDto)
        {
            var result = await _firstSubCategoryService.EditAsync(firstSubCategoryDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _firstSubCategoryService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("is-name-exists")]
        public async Task<IActionResult> IsNameExists([FromBody] FirstSubCategoryDto firstSubCategoryDto)
        {
            var result = await _firstSubCategoryService.IsNameExistsAsync(firstSubCategoryDto);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}