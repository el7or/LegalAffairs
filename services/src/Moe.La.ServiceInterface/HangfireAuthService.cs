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
using System.Security.Claims;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class HangfireAuthService : IHangfireAuthService
    {
        private readonly ILogger<HangfireAuthService> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IHangfireAuthRepository _hangfireAuthRepository;


        public HangfireAuthService(ILogger<HangfireAuthService> logger,
            UserManager<AppUser> userManager,
            IHangfireAuthRepository hangfireAuthRepository,
            IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions)
        {
            _logger = logger;
            _userManager = userManager;
            _hangfireAuthRepository = hangfireAuthRepository;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<ReturnResult<string>> Login(CredentialsDto credentials)
        {
            try
            {
                // get the user to verifty
                var userToVerify = await _hangfireAuthRepository.GetAdminByUserName(credentials.UserName);
                var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);

                if (userToVerify == null || identity == null)
                {
                    return new ReturnResult<string>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status401Unauthorized,
                        ErrorList = new List<string> { "اسم المستخدم او كلمة المرور غير صحيحة" }
                    };
                }
                else if (!userToVerify.Enabled)
                {
                    return new ReturnResult<string>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatuses.Status401Unauthorized,
                        ErrorList = new List<string> { "تم ايقاف المستخدم" }
                    };
                }

                // Get the roles for the user
                var roles = await _userManager.GetRolesAsync(userToVerify);

                // get roles departments for the user
                var user = new UserDto();
                Mapper.Map(userToVerify, user);

                user.Roles = roles;

                var expires = DateTime.UtcNow.AddMinutes(50);

                // jwt object
                var response = await _jwtFactory.GenerateEncodedToken(
                        credentials.UserName,
                        userToVerify.FirstName + " " + userToVerify.LastName,
                        userToVerify.JobTitle != null ? userToVerify.JobTitle.Name : "",
                        userToVerify.Picture != null ? userToVerify.Picture : "",
                        identity,
                        roles, new List<string>(), new List<string>(), "");

                return new ReturnResult<string>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ReturnResult<string>
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

    }
}
