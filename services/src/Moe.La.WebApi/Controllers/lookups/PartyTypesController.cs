//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Moe.La.Core.Dtos;
//using Moe.La.Core.Entities;
//using Moe.La.Core.Interfaces.Services;
//using System.Threading.Tasks;

//namespace Moe.La.WebApi.Controllers
//{
//    [Route("api/party-types")]
//    [Authorize]

//    public class PartyTypesController : ControllerBase
//    {
//        private readonly IPartyTypeService _partyTypeService;

//        public PartyTypesController(IPartyTypeService PartyTypeService)
//        {
//            _partyTypeService = PartyTypeService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll(QueryObject queryObject)
//        {
//            var result = await _partyTypeService.GetAllAsync(queryObject);
//            return StatusCode((int)result.StatusCode, result);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get(int id)
//        {
//            var result = await _partyTypeService.GetAsync(id);
//            return StatusCode((int)result.StatusCode, result);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] PartyTypeDto PartyTypeDto)
//        {
//            var result = await _partyTypeService.AddAsync(PartyTypeDto);
//            return StatusCode((int)result.StatusCode, result);
//        }

//        [HttpPut]
//        public async Task<IActionResult> Update([FromBody] PartyTypeDto PartyTypeDto)
//        {
//            var result = await _partyTypeService.EditAsync(PartyTypeDto);
//            return StatusCode((int)result.StatusCode, result);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var result = await _partyTypeService.RemoveAsync(id);
//            return StatusCode((int)result.StatusCode, result);
//        }
//    }
//}
