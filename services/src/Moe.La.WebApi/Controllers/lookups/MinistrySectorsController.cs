using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/ministry-sectors")]
    [Authorize]

    public class MinistrySectorsController : ControllerBase
    {
        private readonly IMinistrySectorService _ministrySectorService;

        public MinistrySectorsController(IMinistrySectorService ministrySectorService)
        {
            _ministrySectorService = ministrySectorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(QueryObject queryObject)
        {
            var result = await _ministrySectorService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _ministrySectorService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MinistrySectorDto MinistrySectorDto)
        {

            var result = await _ministrySectorService.AddAsync(MinistrySectorDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MinistrySectorDto MinistrySectorDto)
        {

            var result = await _ministrySectorService.EditAsync(MinistrySectorDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = Guid.Parse(User.FindFirst("id").Value);

            var result = await _ministrySectorService.RemoveAsync(id, userId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("is-name-exists/{name}")]
        public async Task<IActionResult> IsNameExists(string name)
        {
            var result = await _ministrySectorService.IsNameExistsAsync(name);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
