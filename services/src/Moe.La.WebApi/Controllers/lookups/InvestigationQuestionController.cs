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
    [Route("api/investigation-questions")]
    [Authorize]
    public class InvestigationQuestionController : ControllerBase
    {
        private readonly IInvestigationQuestionService _InvestigationQuestionService;

        public InvestigationQuestionController(IInvestigationQuestionService InvestigationQuestionService)
        {
            _InvestigationQuestionService = InvestigationQuestionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(InvestigationQuestionQueryObject queryObject)
        {
            var result = await _InvestigationQuestionService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _InvestigationQuestionService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InvestigationQuestionDto InvestigationQuestionDto)
        {
            var result = await _InvestigationQuestionService.AddAsync(InvestigationQuestionDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InvestigationQuestionDto InvestigationQuestionDto)
        {
            var result = await _InvestigationQuestionService.EditAsync(InvestigationQuestionDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _InvestigationQuestionService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("change-status")]
        public async Task<IActionResult> ChangeStatus([FromBody] InvestigationQuestionChangeStatusDto questionChangeStatusDto)
        {
            var result = await _InvestigationQuestionService.ChangeQuestionStatusAsync(questionChangeStatusDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("question-statuses")]
        public IActionResult GetQuestionsStatuses()
        {
            return Ok(EnumExtensions.GetValues<InvestigationQuestionStatuses>());
        }

        [HttpPost("is-name-exists")]
        public async Task<IActionResult> IsNameExists([FromBody] string question)
        {
            var result = await _InvestigationQuestionService.IsNameExistAsync(question);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
