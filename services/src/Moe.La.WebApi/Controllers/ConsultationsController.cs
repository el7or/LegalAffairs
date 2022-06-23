using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Common.Extensions;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/consultations")]
    [Authorize]
    public class ConsultationsController : ControllerBase
    {
        private readonly IConsultationService _consultationService;
        private readonly IAuthorizationService _authorizationService;
        private AuthorizationResult hasConfidentialAccess;
        public ConsultationsController(IConsultationService consultationService, IAuthorizationService authorizationService)
        {
            _consultationService = consultationService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(ConsultationQueryObject queryObject)
        {
            hasConfidentialAccess = await _authorizationService.AuthorizeAsync(HttpContext.User, "MoamlatConfedential");
            queryObject.HasConfidentialAccess = hasConfidentialAccess.Succeeded;
            var result = await _consultationService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _consultationService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ConsultationDto consultationDto)
        {
            var result = await _consultationService.AddAsync(consultationDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ConsultationDto consultationDto)
        {
            var result = await _consultationService.EditAsync(consultationDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("consultation-status")]
        public IActionResult GetConsultationStatus()
        {
            return Ok(EnumExtensions.GetValues<ConsultationStatus>());
        }

        [HttpPost("consultation-review")]
        public async Task<IActionResult> ConsultationReview([FromBody] ConsultationReviewDto consultationReviewDto)
        {
            var result = await _consultationService.ConsultationReview(consultationReviewDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("delete-visual/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _consultationService.DeleteVisualAsync(Id);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
