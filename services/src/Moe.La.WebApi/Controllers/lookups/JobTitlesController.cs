using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/jobs")]
    [Authorize]

    public class JobTitlesController : ControllerBase
    {
        private readonly IJobTitleService _jobTitleService;

        public JobTitlesController(IJobTitleService jobTitleService)
        {
            _jobTitleService = jobTitleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(QueryObject queryObject)
        {
            var result = await _jobTitleService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _jobTitleService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] JobTitleDto JobTitleDto)
        {
            var result = await _jobTitleService.AddAsync(JobTitleDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] JobTitleDto JobTitleDto)
        {
            var result = await _jobTitleService.EditAsync(JobTitleDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _jobTitleService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("is-name-exists/{name}")]
        public async Task<IActionResult> IsNameExists(string name)
        {
            var result = await _jobTitleService.IsNameExistsAsync(name);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
