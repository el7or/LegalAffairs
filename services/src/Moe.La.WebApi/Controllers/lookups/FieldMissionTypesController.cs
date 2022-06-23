//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Moe.La.Core.Dtos;
//using Moe.La.Core.Entities;
//using Moe.La.Core.Interfaces.Services;
//using System.Threading.Tasks;

//namespace Moe.La.WebApi.Controllers
//{
//    [Route("api/field-mission-types")]
//    [Authorize]

//    public class FieldMissionTypesController : ControllerBase
//    {
//        private readonly IFieldMissionTypeService _fieldMissionTypeService;

//        public FieldMissionTypesController(IFieldMissionTypeService FieldMissionTypeService)
//        {
//            _fieldMissionTypeService = FieldMissionTypeService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll(QueryObject queryObject)
//        {
//            var result = await _fieldMissionTypeService.GetAllAsync(queryObject);
//            return StatusCode((int)result.StatusCode, result);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get(int id)
//        {
//            var result = await _fieldMissionTypeService.GetAsync(id);
//            return StatusCode((int)result.StatusCode, result);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] FieldMissionTypeDto FieldMissionTypeDto)
//        {
//            var result = await _fieldMissionTypeService.AddAsync(FieldMissionTypeDto);
//            return StatusCode((int)result.StatusCode, result);
//        }

//        [HttpPut]
//        public async Task<IActionResult> Update([FromBody] FieldMissionTypeDto FieldMissionTypeDto)
//        {
//            var result = await _fieldMissionTypeService.EditAsync(FieldMissionTypeDto);
//            return StatusCode((int)result.StatusCode, result);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var result = await _fieldMissionTypeService.RemoveAsync(id);
//            return StatusCode((int)result.StatusCode, result);
//        }
//    }
//}
