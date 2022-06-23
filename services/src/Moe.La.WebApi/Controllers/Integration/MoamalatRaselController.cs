using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/integration/moamala-log")]
    [AllowAnonymous]

    public class MoamalatRaselController : ControllerBase
    {
        private readonly IMoamalatRaselService _moamalaRaselService;

        public MoamalatRaselController(IMoamalatRaselService moamalaRaselService)
        {
            _moamalaRaselService = moamalaRaselService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(MoamalatRaselQueryObject queryObject)
        {
            var result = await _moamalaRaselService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _moamalaRaselService.GetAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MoamalaRaselDto moamalaRaselDto)
        {
            var result = await _moamalaRaselService.AddAsync(moamalaRaselDto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ReceiveAsync(int id)
        {
            var result = await _moamalaRaselService.ReceiveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _moamalaRaselService.RemoveAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
