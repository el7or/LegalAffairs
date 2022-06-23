using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/provinces")]
    [Authorize]
    public class ProvincesController : ControllerBase
    {
        private readonly IProvinceService _provincesService;

        public ProvincesController(IProvinceService provincesService)
        {
            _provincesService = provincesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(ProvinceQueryObject queryObject)
        {
            var result = await _provincesService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _provincesService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProvinceDto provinceDto)
        {
            var result = await _provincesService.AddAsync(provinceDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProvinceDto provinceDto)
        {
            var result = await _provincesService.EditAsync(provinceDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _provincesService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("is-name-exists/{name}")]
        public async Task<IActionResult> IsNameExists(string name)
        {
            var result = await _provincesService.IsNameExistsAsync(name);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}