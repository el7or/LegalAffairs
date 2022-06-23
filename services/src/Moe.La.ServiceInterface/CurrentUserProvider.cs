using Microsoft.AspNetCore.Http;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Linq;
using System.Security.Claims;

namespace Moe.La.ServiceInterface
{
    public class CurrentUserProvider : IUserProvider
    {
        private readonly CurrentUser _currentUser = new CurrentUser();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CurrentUser CurrentUser
        {
            get
            {
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    _currentUser.UserId = Guid.Parse(_httpContextAccessor.HttpContext.User.FindFirst("id").Value);
                    _currentUser.IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
                    //_currentUser.HostName = Dns.GetHostEntry(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress).HostName;

                    var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext?.User?.Identity;

                    if (claimsIdentity != null)
                    {
                        // get user name
                        var userName = claimsIdentity.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                        if (userName != null)
                        {
                            _currentUser.UserName = userName.Value;
                        }

                        // get general management
                        var Branch = claimsIdentity.Claims.SingleOrDefault(c => c.Type == "Branch");
                        if (!string.IsNullOrEmpty(Branch?.Value))
                        {
                            _currentUser.BranchId = Convert.ToInt32(Branch.Value);
                        }

                        // get user roles
                        var roles = claimsIdentity.Claims.ToList().Where(r => r.Type == ClaimsIdentity.DefaultRoleClaimType);
                        foreach (var item in roles)
                        {
                            _currentUser.Roles.Add(item.Value);
                        }

                        // get departments
                        var departments = claimsIdentity.Claims.ToList().Where(c => c.Type == "Department");
                        foreach (var item in departments)
                        {
                            _currentUser.Departments.Add(Convert.ToInt32(item.Value));
                        }

                        // get permissions
                        var permissions = claimsIdentity.Claims.ToList().Where(c => c.Type == "Permission");
                        foreach (var item in permissions)
                        {
                            _currentUser.Permissions.Add(item.Value);
                        }
                    }
                }

                return _currentUser;
            }
        }
    }
}
