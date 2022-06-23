using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/main-categories")]
    [Authorize]
    public class MainCategoriesController : ControllerBase
    {
        private readonly IMainCategoryService _mainCategoryService;

        public MainCategoriesController(IMainCategoryService mainCategoryService)
        {
            _mainCategoryService = mainCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(MainCategoryQueryObject queryObject)
        {
            var result = await _mainCategoryService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mainCategoryService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MainCategoryDto mainCategoryDto)
        {
            var result = await _mainCategoryService.AddAsync(mainCategoryDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MainCategoryDto mainCategoryDto)
        {
            var result = await _mainCategoryService.EditAsync(mainCategoryDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mainCategoryService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("is-name-exists")]
        public async Task<IActionResult> IsNameExists([FromBody] MainCategoryDto mainCategoryDto)
        {
            var result = await _mainCategoryService.IsNameExistsAsync(mainCategoryDto);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}