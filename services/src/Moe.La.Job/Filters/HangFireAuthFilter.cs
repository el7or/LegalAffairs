using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Moe.La.Core.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Moe.La.Job.Filters
{
    public class HangFireAuthFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            if (httpContext.Session.GetString("token") != null)
            {
                var token = new JwtSecurityTokenHandler().ReadJwtToken(httpContext.Session.GetString("token"));

                if (token.Claims.Any(n => n.Type == "roles" && n.Value == ApplicationRolesConstants.Admin.Name))
                    return true;
            }
            return false;
        }
    }
}
