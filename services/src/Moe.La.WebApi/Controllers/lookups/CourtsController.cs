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
    [Route("api/courts")]
    [Authorize]

    public class CourtsController : ControllerBase
    {
        private readonly ICourtService _courtService;

        public CourtsController(ICourtService courtService)
        {
            _courtService = courtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CourtQueryObject queryObject)
        {
            var result = await _courtService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _courtService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourtDto courtDto)
        {
            var result = await _courtService.AddAsync(courtDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CourtDto courtDto)
        {
            var result = await _courtService.EditAsync(courtDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _courtService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpGet("litigation-types")]
        public IActionResult GetCourtTypes()
        {
            return Ok(EnumExtensions.GetValues<LitigationTypes>());
        }

        [HttpGet("court-categories")]
        public IActionResult GetCourtCategories()
        {
            return Ok(EnumExtensions.GetValues<CourtCategories>());
        }

        [HttpPost("is-name-exists")]
        public async Task<IActionResult> IsNameExists([FromBody] CourtDto courtDto)
        {
            var result = await _courtService.IsNameExistsAsync(courtDto);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
