using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IRefreshTokenRepository _refreshTokenRepository;


        public AuthService(ILogger<AuthService> logger,
            UserManager<AppUser> userManager,
            IUserService userService,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions)
        {
            _logger = logger;
            _userManager = userManager;
            _userService = userService;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<ReturnResult<object>> Login(CredentialsDto credentials, string rule_name = null)
        {
            try
            {
                var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
                if (identity == null)
                {
                    return new ReturnResult<object>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status401Unauthorized,
                        ErrorList = new List<string> { "اسم المستخدم او كلمة المرور غير صحيحة" }
                    };

                }

                // get the user to verifty
                var userToVerify = _userService.GetByUserName(credentials.UserName).Result.Data;

                if (!userToVerify.Enabled)
                    return new ReturnResult<object>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status401Unauthorized,
                        ErrorList = new List<string> { "تم ايقاف المستخدم" }
                    };

                if (userToVerify.IsDeleted)
                    return new ReturnResult<object>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status401Unauthorized,
                        ErrorList = new List<string> { "تم حذف المستخدم" }
                    };
                // Get the roles for the user
                var roles = await _userManager.GetRolesAsync(userToVerify);

                if (rule_name != null)
                {
                    if (!roles.Contains(rule_name))
                    {
                        return new ReturnResult<object>
                        {
                            IsSuccess = false,
                            StatusCode = HttpStatuses.Status401Unauthorized,
                            ErrorList = new List<string> { "المستخدم لايملك صلاحية الدخول" }
                        };

                    }
                }

                // get roles Departments for the user
                var user = new UserDto();
                Mapper.Map(userToVerify, user);
                var userRoleDepartments = _userService.GetUserRolesDepartmentsAsync(user).Result.Data;

                //  Get the claims for the user
                List<string> permissions = new List<string>();
                string branch = string.Empty;
                List<string> departments = new List<string>();
                var userClaims = await _userManager.GetClaimsAsync(userToVerify);

                foreach (var item in userClaims)
                {
                    if (item.Type == "Permission")
                    {
                        permissions.Add(item.Value);
                    }
                    if (item.Type == "Branch")
                    {
                        branch = item.Value;
                    }
                    if (item.Type == "Department")
                    {
                        departments.Add(item.Value);
                    }
                }

                // set userDto to caching
                user.UserRoleDepartments = userRoleDepartments;
                user.Roles = roles;
                user.Claims = permissions;

                //generate refresh token to sent with it 
                RefreshTokenDto refreshToken = _jwtFactory.GenerateRefreshToken();
                refreshToken.UserId = userToVerify.Id;
                var dbRefreshToken = _refreshTokenRepository.AddAsync(refreshToken).Result;
                var expires = DateTime.UtcNow.AddMinutes(1);

                // jwt object
                var response = new
                {
                    id = identity.Claims.Single(c => c.Type == "id").Value,
                    auth_token = await _jwtFactory.GenerateEncodedToken(
                        credentials.UserName,
                        userToVerify.FirstName + " " + userToVerify.LastName,
                        userToVerify.JobTitle != null ? userToVerify.JobTitle.Name : "",
                        userToVerify.Picture != null ? userToVerify.Picture : "",
                        identity,
                        roles, permissions, departments, branch),
                    refresh_token = refreshToken.Token,
                    roles_departments = userRoleDepartments,
                    expires_in = (int)_jwtOptions.ValidFor.TotalMinutes
                };

                return new ReturnResult<object>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ReturnResult<object>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." }
                };
            }
        }


        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName.ToString()) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName.ToString());

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                // Get the roles for the user
                var roles = await _userManager.GetRolesAsync(userToVerify);

                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName.ToString(), userToVerify.Id.ToString()));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        public async Task<ReturnResult<object>> RefreshToken(string Token)
        {
            var refreshTokenList = await _refreshTokenRepository.GetAllAsync();
            var refreshToken = refreshTokenList.Where(x => x.Token == Token).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = _jwtFactory.GenerateRefreshToken();
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            //Get identity
            var identity = _jwtFactory.GenerateClaimsIdentity(user.UserName, user.Id.ToString());

            // Get the roles for the user
            var roles = await _userManager.GetRolesAsync(user);

            //  Get the claims for the user
            List<string> permissions = new List<string>();
            string branch = string.Empty;
            List<string> departments = new List<string>();
            var userClaims = await _userManager.GetClaimsAsync(user);

            foreach (var item in userClaims)
            {
                if (item.Type == "Permission")
                {
                    permissions.Add(item.Value);
                }
                if (item.Type == "Branch")
                {
                    branch = item.Value;
                }
                if (item.Type == "Department")
                {
                    departments.Add(item.Value);
                }
            }

            // jwt object
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await _jwtFactory.GenerateEncodedToken(
                    user.UserName,
                    user.FirstName + " " + user.LastName,
                    user.JobTitle != null ? user.JobTitle.Name : "",
                    user.Picture != null ? user.Picture : "",
                    identity,
                    roles, permissions, departments, branch),
                refresh_token = newRefreshToken.Token,
                expires_in = (int)_jwtOptions.ValidFor.TotalMinutes
            };

            return new ReturnResult<object>
            {
                IsSuccess = true,
                StatusCode = HttpStatuses.Status200OK,
                Data = response
            };
        }

        public async Task<ReturnResult<bool>> RevokeToken(string token)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null)
            {
                return new ReturnResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = false
                };

            }
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive)
            {
                return new ReturnResult<bool>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = false
                };
            }
            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new ReturnResult<bool>
            {
                IsSuccess = true,
                StatusCode = HttpStatuses.Status200OK,
                Data = true
            };
        }
    }
}
