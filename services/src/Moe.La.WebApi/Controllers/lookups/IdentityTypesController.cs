using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/identity-types")]
    [Authorize]

    public class IdentityTypesController : ControllerBase
    {
        private readonly IIdentityTypeService _identityTypeService;

        public IdentityTypesController(IIdentityTypeService identityTypeService)
        {
            _identityTypeService = identityTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(QueryObject queryObject)
        {
            var result = await _identityTypeService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _identityTypeService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IdentityTypeDto identityTypeDto)
        {
            var result = await _identityTypeService.AddAsync(identityTypeDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] IdentityTypeDto identityTypeDto)
        {
            var result = await _identityTypeService.EditAsync(identityTypeDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _identityTypeService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("is-name-exists/{name}")]
        public async Task<IActionResult> IsNameExists(string name)
        {
            var result = await _identityTypeService.IsNameExistsAsync(name);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
