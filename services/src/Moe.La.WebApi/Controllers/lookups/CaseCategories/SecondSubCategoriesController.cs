using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/second-sub-categories")]
    [Authorize]
    public class SecondSubCategoriesController : ControllerBase
    {
        private readonly ISecondSubCategoryService _secondSubCategoryService;

        public SecondSubCategoriesController(ISecondSubCategoryService secondSubCategoryService)
        {
            _secondSubCategoryService = secondSubCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(SecondSubCategoryQueryObject queryObject)
        {
            var result = await _secondSubCategoryService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _secondSubCategoryService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SecondSubCategoryDto secondSubCategoryDto)
        {
            var result = await _secondSubCategoryService.AddAsync(secondSubCategoryDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SecondSubCategoryDto secondSubCategoryDto)
        {
            var result = await _secondSubCategoryService.EditAsync(secondSubCategoryDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _secondSubCategoryService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("is-name-exists")]
        public async Task<IActionResult> IsNameExists([FromBody] SecondSubCategoryDto secondSubCategoryDto)
        {
            var result = await _secondSubCategoryService.IsNameExistsAsync(secondSubCategoryDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("change-category-activity")]
        public async Task<IActionResult> changeCatergoryActivity(int SecondSubCategoryId, bool IsActive)
        {
            var result = await _secondSubCategoryService.ChangeCatergoryActivityAsync(SecondSubCategoryId, IsActive);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}