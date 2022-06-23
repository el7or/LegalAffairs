using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/parties")]
    [Authorize]

    public class PartiesController : ControllerBase
    {
        private readonly IPartyService _partyService;

        public PartiesController(IPartyService partyService)
        {
            _partyService = partyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(PartyQueryObject queryObject)
        {
            var result = await _partyService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _partyService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PartyDto PartyDto)
        {
            var result = await _partyService.AddAsync(PartyDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PartyDto PartyDto)
        {
            var result = await _partyService.EditAsync(PartyDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _partyService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("party-exist")]
        public async Task<IActionResult> IsPartyExist([FromBody] PartyDto PartyDto)
        {
            var result = await _partyService.IsPartyExist(PartyDto);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
