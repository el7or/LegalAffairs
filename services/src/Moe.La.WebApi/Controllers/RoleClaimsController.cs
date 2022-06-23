using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers
{
    [Route("api/role-claims")]
    [Authorize]
    public class RoleClaimsController : ControllerBase
    {
        private readonly IRoleClaimService _roleClaimService;

        public RoleClaimsController(IRoleClaimService roleClaimService)
        {
            _roleClaimService = roleClaimService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(QueryObject queryObject)
        {
            var result = await _roleClaimService.GetAllAsync(queryObject);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}