using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/researcher-consultant")]
    [Authorize]

    public class ResearcherConsultantController : ControllerBase
    {
        private readonly IResearchsConsultantService _researcherConsultantService;

        public ResearcherConsultantController(IResearchsConsultantService researcherConsultantService)
        {
            _researcherConsultantService = researcherConsultantService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAll(int id)
        {
            var result = await _researcherConsultantService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet]
        public async Task<IActionResult> Get(ResearcherQueryObject queryObject)
        {
            var result = await _researcherConsultantService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ResearcherConsultantDto researcherConsultantDto)
        {
            var result = await _researcherConsultantService.AddAsync(researcherConsultantDto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
