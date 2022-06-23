using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Moe.La.Core.Constants;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Moe.La.WebApi
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly IUserService _userService;
        private readonly ILogger<ClaimsTransformer> _logger;

        public ClaimsTransformer(IUserService userService, ILogger<ClaimsTransformer> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            try
            {
                if (principal.Identity.IsAuthenticated)
                {
                    // get user from cache
                    var userId = principal.Identity.IsAuthenticated ? principal.Claims.Where(c => c.Type == "id").First().Value : Guid.Empty.ToString();

                    var userDto = await _userService.GetUserRolesDepartmentsAsync(Guid.Parse(userId));

                    //if (!userDto.Enabled)
                    /// TODO


                    // get departments , roles
                    var departmentIds = userDto.Data.Where(s => s.UserId == Guid.Parse(userId))
                                                   .Select(s => new { s.DepartmentId, s.RoleId })
                                                   .ToList();

                    foreach (var item in departmentIds)
                    {
                        if (item.RoleId == ApplicationRolesConstants.DepartmentManager.Code)
                        {
                            // litigation DepartmentManager
                            int litigation = (int)Departments.Litigation;
                            var HaslitigationDepartmentManager = item.DepartmentId.Equals(litigation);

                            if (!principal.HasClaim("DepartmentManager", Convert.ToString(litigation)) && HaslitigationDepartmentManager)
                            {
                                (principal.Identity as ClaimsIdentity).AddClaim(new Claim("DepartmentManager", Convert.ToString(litigation)));
                            }

                            // Investigation DepartmentManager
                            int investigation = (int)Departments.Investigation;
                            var HasInvestigationDepartmentManager = item.DepartmentId.Equals(investigation);

                            if (!principal.HasClaim("DepartmentManager", Convert.ToString(investigation)) && HasInvestigationDepartmentManager)
                            {
                                (principal.Identity as ClaimsIdentity).AddClaim(new Claim("DepartmentManager", Convert.ToString(investigation)));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                _logger.LogError(ex.Message, ex, principal);
            }

            return principal;
        }
    }
}
