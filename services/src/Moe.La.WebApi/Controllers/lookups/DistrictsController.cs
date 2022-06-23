using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/districts")]
    [Authorize]

    public class DistrictsController : ControllerBase
    {
        private readonly IDistrictService _districtService;

        public DistrictsController(IDistrictService districtService)
        {
            _districtService = districtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(DistrictQueryObject queryObject)
        {
            var result = await _districtService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _districtService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DistrictDto districtDto)
        {

            var result = await _districtService.AddAsync(districtDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DistrictDto districtDto)
        {

            var result = await _districtService.EditAsync(districtDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _districtService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("is-name-exists")]
        public async Task<IActionResult> IsNameExists([FromBody] DistrictDto districtDto)
        {
            var result = await _districtService.IsNameExistsAsync(districtDto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
