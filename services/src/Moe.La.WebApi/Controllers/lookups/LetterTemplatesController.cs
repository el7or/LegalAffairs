using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/lettertemplates")]
    [Authorize]
    public class LetterTemplatesController : ControllerBase
    {
        private readonly ILetterTemplateService _letterTemplateService;

        public LetterTemplatesController(ILetterTemplateService letterTemplateService)
        {
            _letterTemplateService = letterTemplateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(TemplateQueryObject queryObject)
        {
            var result = await _letterTemplateService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _letterTemplateService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LetterTemplateDto templateDto)
        {
            var result = await _letterTemplateService.AddAsync(templateDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LetterTemplateDto templateDto)
        {
            var result = await _letterTemplateService.EditAsync(templateDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _letterTemplateService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}