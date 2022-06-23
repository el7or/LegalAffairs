using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private IWebHostEnvironment _host;
        private readonly INotificationService _notificationService;
        private readonly IDistributedCache _cache;
        private readonly IUserProvider _userProvider;

        public UserService(IUserRepository userRepository,
            ILogger<UserService> logger,
            IWebHostEnvironment host,
            INotificationService notificationService,
            IDistributedCache cache,
            IUserProvider userProvider)
        {
            _userRepository = userRepository;
            _logger = logger;
            _host = host;
            _notificationService = notificationService;
            _cache = cache;
            _userProvider = userProvider;
        }

        public CurrentUser CurrentUser => _userProvider?.CurrentUser ?? null;

        public async Task<ReturnResult<QueryResultDto<UserListItemDto>>> GetAllAsync(UserQueryObject queryObject)
        {
            try
            {

                var entities = await _userRepository.GetAllAsync(queryObject);

                return new ReturnResult<QueryResultDto<UserListItemDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, queryObject);

                return new ReturnResult<QueryResultDto<UserListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<UserDetailsDto>> GetAsync(Guid id)
        {
            try
            {
                var entitiy = await _userRepository.GetAsync(id);

                return new ReturnResult<UserDetailsDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);

                return new ReturnResult<UserDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<UserDetailsDto>> GetDepartmentManagerAsync(int departmentId)
        {
            try
            {
                var entitiy = await _userRepository.GetDepartmentManagerAsync(departmentId);

                return new ReturnResult<UserDetailsDto>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, departmentId);

                return new ReturnResult<UserDetailsDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<List<UserDetailsDto>>> GetUsersByRolesAsync(Guid roleId, int? branchId, int? departmetId = null)
        {
            try
            {
                var entities = await _userRepository.GetUsersByRolesAsync(roleId, branchId, departmetId);

                return new ReturnResult<List<UserDetailsDto>>(true, HttpStatuses.Status200OK, entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, roleId);

                return new ReturnResult<List<UserDetailsDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<UserDto>> AddAsync(UserDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new UserValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors);
                }

                bool isPhoneNumberExists = await _userRepository.IsPhoneNumberExistsAsync(model);
                if (isPhoneNumberExists)
                {
                    errors.Add("رقم الجوال موجود مسبقاً لمستخدم آخر.");
                }

                if (model.Roles.Contains(ApplicationRolesConstants.LegalResearcher.Name)
                    && model.Roles.Contains(ApplicationRolesConstants.LegalConsultant.Name))
                {
                    errors.Add("لا يمكن ان يكون المستخدم مستشار وباحث بنفس الوقت.");
                }

                if (model.Roles.Contains(ApplicationRolesConstants.MainBoardHead.Name))
                {
                    bool isMainBoardHeadExist = await _userRepository.IsMainBoardHeadExist(model);
                    if (isMainBoardHeadExist)
                    {
                        errors.Add("يوجد رئيس لجنة رئيسية بالفعل ولا يمكن إضافة اكثر من رئيس لجنة رئيسية.");
                    }
                }

                var userResult = await GetByUserName(model.UserName);
                if (userResult.Data != null)
                {
                    errors.Add("لا يمكن إضافة المستخدم لأنه موجود بالفعل.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<UserDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _userRepository.AddAsync(model);

                // send notification to general manager
                await NotificationAddUser(model);

                return new ReturnResult<UserDto>(true, HttpStatuses.Status201Created, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<UserDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<UserDto>> EditAsync(UserDto model)
        {
            try
            {
                var errors = new List<string>();
                var validationResult = ValidationResult.CheckModelValidation(new UserValidator(), model);

                if (!validationResult.IsValid)
                {
                    errors.AddRange(validationResult.Errors.ToList());
                }

                bool isPhoneNumberExists = await _userRepository.IsPhoneNumberExistsAsync(model);
                if (isPhoneNumberExists)
                {
                    errors.Add("رقم الجوال موجود مسبقاً لمستخدم آخر.");
                }

                if (model.Roles.Contains(ApplicationRolesConstants.LegalResearcher.Name)
                    && model.Roles.Contains(ApplicationRolesConstants.LegalConsultant.Name))
                {
                    errors.Add("لا يمكن ان يكون المستخدم مستشار وباحث بنفس الوقت.");
                }

                if (model.Roles.Contains(ApplicationRolesConstants.MainBoardHead.Name))
                {
                    bool isMainBoardHeadExist = await _userRepository.IsMainBoardHeadExist(model);
                    if (isMainBoardHeadExist)
                    {
                        errors.Add("يوجد رئيس لجنة رئيسية بالفعل ولا يمكن إضافة اكثر من رئيس لجنة رئيسية.");
                    }
                }

                var userDetailsOld = (await GetAsync((Guid)model.Id)).Data;

                // if user Researcher
                var isRelatedResearcher = CheckUserDataHasChanged(userDetailsOld, model, ApplicationRolesConstants.LegalResearcher.Name);
                if (isRelatedResearcher)
                {
                    // is researcher has Related
                    errors.AddRange(await CheckResearcherHasRelated((Guid)model.Id));
                }

                // if user Consultant
                var isRelatedConsultant = CheckUserDataHasChanged(userDetailsOld, model, ApplicationRolesConstants.LegalConsultant.Name);
                if (isRelatedConsultant)
                {
                    // is consultant has related
                    errors.AddRange(await CheckConsultantHasRelated((Guid)model.Id));
                }

                // if user Board Head Senior
                var isRelatedMainBoardHead = CheckUserDataHasChanged(userDetailsOld, model, ApplicationRolesConstants.MainBoardHead.Name);
                if (isRelatedMainBoardHead)
                {
                    bool checkLegalBoardMembersExists = await _userRepository.CheckLegalBoardMembersExists((Guid)model.Id);
                    if (checkLegalBoardMembersExists)
                    {
                        errors.Add("المستخدم رئيس لجنة رئيسية بالفعل.");
                    }
                }

                if (errors.Any())
                {
                    return new ReturnResult<UserDto>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _userRepository.EditAsync(model);

                // send a notification to general managers when edit user  
                await NotificationEditUser(userDetailsOld, model);

                return new ReturnResult<UserDto>(true, HttpStatuses.Status200OK, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, model);

                return new ReturnResult<UserDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> RemoveAsync(Guid userId)
        {
            try
            {
                var errors = new List<string>();

                // is researcher has Related
                errors.AddRange(await CheckResearcherHasRelated(userId));

                // is consultant has related
                errors.AddRange(await CheckConsultantHasRelated(userId));

                bool checkLegalBoardMembersExists = await _userRepository.CheckLegalBoardMembersExists(userId);

                if (checkLegalBoardMembersExists)
                {
                    errors.Add("المستخدم موجود ضمن تشكيل اللجان.");
                }

                if (errors.Any())
                {
                    return new ReturnResult<bool>(false, HttpStatuses.Status400BadRequest, errors);
                }

                await _userRepository.RemoveAsync(userId);

                return new ReturnResult<bool>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatuses.Status200OK,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, userId);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ICollection<RoleListItemDto>>> GetUserRolesAsync(Guid userId)
        {
            try
            {
                var entitiy = await _userRepository.GetUserRolesAsync(userId);

                return new ReturnResult<ICollection<RoleListItemDto>>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, userId);

                return new ReturnResult<ICollection<RoleListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<AppUser>> GetByUserName(string userName)
        {
            try
            {
                var entitiy = await _userRepository.GetByUserName(userName);

                return new ReturnResult<AppUser>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, userName);

                return new ReturnResult<AppUser>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ICollection<ConsultantDto>>> GetConsultants(string userName, int? legalBoardId)
        {
            try
            {
                var entitiy = await _userRepository.GetConsultants(userName, legalBoardId);

                return new ReturnResult<ICollection<ConsultantDto>>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, userName);

                return new ReturnResult<ICollection<ConsultantDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> EnabledUserAsync(Guid userId, bool enabled)
        {
            try
            {
                var user = await _userRepository.EnabledUserAsync(userId, enabled);

                // send a notification to general managers when disabled user  
                await NotificationDisabledUser(user);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, userId);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ICollection<UserRoleDepartmentDto>>> GetUserRolesDepartmentsAsync(UserDto user)
        {
            try
            {
                var entitiy = await _userRepository.GetUserRolesDepartmentsAsync((Guid)user.Id);

                return new ReturnResult<ICollection<UserRoleDepartmentDto>>(true, HttpStatuses.Status200OK, entitiy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, user.Id);

                return new ReturnResult<ICollection<UserRoleDepartmentDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<ICollection<UserRoleDepartmentDto>>> GetUserRolesDepartmentsAsync(Guid userId)
        {
            try
            {
                var user = _userRepository.GetUserDetailsAsync(userId).Result;

                var result = await GetUserRolesDepartmentsAsync(user);

                return new ReturnResult<ICollection<UserRoleDepartmentDto>>(true, HttpStatuses.Status200OK, result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, userId);

                return new ReturnResult<ICollection<UserRoleDepartmentDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        private bool CheckUserDataHasChanged(UserDetailsDto userDetailsOld, UserDto userDto, string userRoleName)
        {
            bool isChanged = false;
            bool isBranchChanged = false;
            bool isRolesChanged = false;
            bool isDepartmentChanged = false;

            //change general management
            var branchOld = userDetailsOld.Branch.Id;
            if (branchOld != userDto.BranchId)
            {
                isBranchChanged = true;
            }

            // remove role
            var rolesOld = userDetailsOld.Roles.Select(r => r.Name);
            var removedRoles = rolesOld.Except(userDto.Roles);
            if (removedRoles.Count() > 0)
            {
                foreach (var roleName in removedRoles)
                {
                    if (roleName == userRoleName)
                    {
                        isRolesChanged = true;
                    }
                }
            }

            // change department
            var roleDepartmentOld = userDetailsOld.UserRoleDepartments.Select(s => new { s.RoleId, s.DepartmentId });
            var removedDepartment = roleDepartmentOld.Except(userDto.UserRoleDepartments.Select(s => new { s.RoleId, s.DepartmentId }));
            if (removedDepartment.Count() > 0)
            {
                // check removed roles on user
                foreach (var removedDepartments in removedDepartment)
                {
                    var isRoleRemoved = userDetailsOld.Roles.Where(r => r.Id == removedDepartments.RoleId && r.Name == userRoleName).Any();
                    if (isRoleRemoved)
                    {
                        isDepartmentChanged = true;
                    }
                }
            }

            if ((isBranchChanged && userDto.Roles.Contains(userRoleName)) || isRolesChanged || isDepartmentChanged)
                isChanged = true;

            return isChanged;
        }

        private async Task<List<string>> CheckResearcherHasRelated(Guid userId)
        {
            var errors = new List<string>();
            bool checkResearcherExists = await _userRepository.CheckResearcherExists(userId);
            if (checkResearcherExists)
            {
                errors.Add("الرجاء حذف الباحث اولا من صفحة ربط الباحثين بالمستشار.");
            }

            bool checkCaseResearcherExists = await _userRepository.CheckCaseResearcherExists(userId);
            if (checkCaseResearcherExists)
            {
                errors.Add("يوجد قضايا للباحث.");
            }

            bool checkHearingsResearcherExists = await _userRepository.CheckHearingsResearcherExists(userId);
            if (checkHearingsResearcherExists)
            {
                errors.Add("يوجد جلسات للباحث.");
            }

            return errors;
        }

        private async Task<List<string>> CheckConsultantHasRelated(Guid userId)
        {
            var errors = new List<string>();
            bool checkConsultantExists = await _userRepository.CheckConsultantExists(userId);
            if (checkConsultantExists)
            {
                errors.Add("الرجاء حذف المستشار اولا من صفحة ربط الباحثين بالمستشار.");
            }

            bool checkLegalBoardMembersExists = await _userRepository.CheckLegalBoardMembersExists(userId);
            if (checkLegalBoardMembersExists)
            {
                errors.Add("المستخدم عضو ضمن تشكيل اللجان.");
            }

            return errors;
        }

        public async Task<ReturnResult<bool>> AssignRoleToUserAsync(Guid userId, string roles)
        {
            try
            {
                await _userRepository.AssignRoleToUserAsync(userId, roles);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, userId);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> IsInRole(Guid userId, Guid roleId)
        {
            try
            {
                var isInRole = await _userRepository.IsInRole(userId, roleId);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, isInRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, userId);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> CheckUserEnabled(Guid userId)
        {
            try
            {
                var enabled = await _userRepository.CheckUserEnabled(userId);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, enabled);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, userId);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> CheckSameLegalDepartment(Guid researcherId, Guid consultantId)
        {
            try
            {
                var enabled = await _userRepository.CheckSameLegalDepartment(researcherId, consultantId);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, enabled);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, researcherId);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        public async Task<ReturnResult<bool>> CheckJobTitleExists(int jobTitleId)
        {
            try
            {
                var data = await _userRepository.CheckJobTitleExists(jobTitleId);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, jobTitleId);

                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { "حدث خطأ أثناء تنفيذ العملية، يرجى المحاولة لاحقاً." });
            }
        }

        #region Notifications

        public async Task NotificationAddUser(UserDto userDto)
        {
            var userDetails = GetAsync((Guid)userDto.Id).Result.Data;
            var BranchManagers = await _userRepository.GetUsersByRolesAsync(ApplicationRolesConstants.BranchManager.Code, userDto.BranchId);

            // send a notification to general managers when add user
            var notificationDto = new NotificationDto()
            {
                Id = 0,
                Text = "تم إضافة مستخدم جديد في " + userDetails.Branch.Name,
                URL = "/users/view/" + userDetails.Id,
                SendEmailMessage = false,
                UserIds = BranchManagers.Select(m => m.Id).ToList()
            };

            await _notificationService.AddAsync(notificationDto);
            ////
            // send a notification to new user when add user
            var users = new List<Guid>();
            users.Add(userDetails.Id);

            var notificationToUser = new NotificationDto()
            {
                Id = 0,
                Text = "تم اضافتك كمستخدم لادارة الشؤون القانونية >> " + userDetails.Branch.Name + " << ",
                UserIds = users
            };

            await _notificationService.AddAsync(notificationToUser);
            ////
        }

        public async Task NotificationDisabledUser(UserDto userDto)
        {
            if (!userDto.Enabled)
            {
                var userIds = new List<Guid>();

                // send a notification to general managers
                userIds.AddRange((await _userRepository.GetUsersByRolesAsync(ApplicationRolesConstants.BranchManager.Code, userDto.BranchId)).Select(m => m.Id).ToList());

                // send notification to department manager
                var userDetails = (await GetAsync((Guid)userDto.Id)).Data;
                if (userDetails.UserRoleDepartments.Count() > 0)
                {
                    foreach (var userRoleDepartment in userDetails.UserRoleDepartments)
                    {
                        userIds.AddRange((await _userRepository.GetUsersByRolesAsync(ApplicationRolesConstants.DepartmentManager.Code, userDto.BranchId, userRoleDepartment.DepartmentId)).Select(m => m.Id).ToList());
                    }
                }

                var notificationDto = new NotificationDto()
                {
                    Id = 0,
                    Text = "تم تعطيل الموظف " + "<< " + userDto.FirstName + " " + userDto.LastName + " >>",
                    URL = "/users/view/" + userDto.Id,
                    Type = NotificationTypes.Danger,
                    SendEmailMessage = false,
                    UserIds = userIds.Distinct().ToList()
                };

                await _notificationService.AddAsync(notificationDto);
            }
        }

        public async Task NotificationEditUser(UserDetailsDto userDetailsOld, UserDto userDto)
        {
            // send a notification to general managers when disabled user  
            await NotificationDisabledUser(userDto);

            // send a notification to general managers & user when removed roles on user
            await NotificationRolesRemoved(userDetailsOld, userDto);

            // send a notification to general managers & user when change general managment on user
            await NotificationChangeBranch(userDetailsOld, userDto);

            // send a notification to general managers & user when removed department on user
            await NotificationDepartmentRemoved(userDetailsOld, userDto);
        }

        private async Task NotificationRolesRemoved(UserDetailsDto userDetailsOld, UserDto userDto)
        {
            var rolesOld = userDetailsOld.Roles.Select(r => r.Name);
            var removedRoles = rolesOld.Except(userDto.Roles);

            if (removedRoles.Count() > 0)
            {
                // send a notification to general managers & user when removed roles on user
                foreach (var roleName in removedRoles)
                {
                    var userIds = new List<Guid>();

                    var roleDetails = userDetailsOld.Roles.Where(r => r.Name == roleName);
                    bool isRoleDistributable = roleDetails.Select(r => r.IsDistributable).FirstOrDefault();
                    if (isRoleDistributable)
                    {
                        var departmentId = userDetailsOld.UserRoleDepartments.Where(r => r.RoleName == roleName).Select(s => s.DepartmentId).FirstOrDefault();
                        userIds.AddRange((await _userRepository.GetUsersByRolesAsync(ApplicationRolesConstants.DepartmentManager.Code, userDto.BranchId, departmentId)).Select(m => m.Id).ToList());
                    }
                    // in all cases, send notification to general manager
                    userIds.AddRange((await _userRepository.GetUsersByRolesAsync(ApplicationRolesConstants.BranchManager.Code, userDto.BranchId)).Select(m => m.Id).ToList());

                    // add user to list
                    userIds.Add((Guid)userDto.Id);

                    var notificationDto = new NotificationDto()
                    {
                        Id = 0,
                        Text = "تم رفع الدور الوظيفي "
                        + "<< " + roleDetails.Select(r => r.NameAr).FirstOrDefault() + " >>" +
                        " عن الموظف " + "<< " + userDto.FirstName + " " + userDto.LastName + " >>",
                        URL = "/users/view/" + userDto.Id,
                        Type = NotificationTypes.Danger,
                        SendEmailMessage = false,
                        UserIds = userIds.Distinct().ToList()
                    };

                    await _notificationService.AddAsync(notificationDto);
                    //////
                }
            }
        }

        private async Task NotificationChangeBranch(UserDetailsDto userDetailsOld, UserDto userDto)
        {
            var branchOld = userDetailsOld.Branch.Id;

            if (branchOld != userDto.BranchId)
            {
                var userIds = new List<Guid>();

                // send notification to department managers
                if (userDetailsOld.UserRoleDepartments.Count() > 0)
                {
                    foreach (var userRoleDepartment in userDetailsOld.UserRoleDepartments)
                    {
                        userIds.AddRange((await _userRepository.GetUsersByRolesAsync(ApplicationRolesConstants.DepartmentManager.Code, userDto.BranchId, userRoleDepartment.DepartmentId)).Select(m => m.Id).ToList());
                    }
                }

                // send a notification to general managers & user when removed roles on user
                userIds.AddRange((await _userRepository.GetUsersByRolesAsync(ApplicationRolesConstants.BranchManager.Code, branchOld)).Select(m => m.Id).ToList());

                // add user to list
                userIds.Add((Guid)userDto.Id);

                var notificationDto = new NotificationDto()
                {
                    Id = 0,
                    Text = "تم نقل الموظف "
                    + "<< " + userDto.FirstName + " " + userDto.LastName + " >>" +
                    " إلى إدارة تعليم أخرى ",
                    URL = "/users/view/" + userDto.Id,
                    Type = NotificationTypes.Danger,
                    SendEmailMessage = false,
                    UserIds = userIds.Distinct().ToList()
                };

                await _notificationService.AddAsync(notificationDto);
            }
        }

        private async Task NotificationDepartmentRemoved(UserDetailsDto userDetailsOld, UserDto userDto)
        {
            var roleDepartmentOld = userDetailsOld.UserRoleDepartments.Select(s => new { s.RoleName, s.DepartmentId });

            var removedDepartment = roleDepartmentOld.Except(userDto.UserRoleDepartments.Select(s => new { s.RoleName, s.DepartmentId }));

            if (removedDepartment.Count() > 0)
            {
                // send a notification to general managers & user when removed roles on user
                foreach (var removedDepartments in removedDepartment)
                {
                    var userIds = new List<Guid>();

                    var roleDetails = userDetailsOld.Roles.Where(r => r.Name == removedDepartments.RoleName);
                    bool isRoleDistributable = roleDetails.Select(r => r.IsDistributable).FirstOrDefault();
                    if (isRoleDistributable)
                    {
                        userIds.AddRange((await _userRepository.GetUsersByRolesAsync(ApplicationRolesConstants.DepartmentManager.Code, userDto.BranchId, removedDepartments.DepartmentId)).Select(m => m.Id).ToList());
                    }
                    // in all cases, send notification to general manager
                    userIds.AddRange((await _userRepository.GetUsersByRolesAsync(ApplicationRolesConstants.BranchManager.Code, userDto.BranchId)).Select(m => m.Id).ToList());

                    // add user to list
                    userIds.Add((Guid)userDto.Id);
                    var departmentName = userDetailsOld.UserRoleDepartments
                        .Where(r => r.RoleName == removedDepartments.RoleName && r.DepartmentId == removedDepartments.DepartmentId)
                        .Select(s => s.DepartmentId).FirstOrDefault();

                    var notificationDto = new NotificationDto()
                    {
                        Id = 0,
                        Text = "تم رفع الموظف "
                        + "<< " + userDto.FirstName + " " + userDto.LastName + " >>" +
                        " من الإدارة التخصصية " + "<< " + departmentName + " >>",
                        URL = "/users/view/" + userDto.Id,
                        Type = NotificationTypes.Danger,
                        SendEmailMessage = false,
                        UserIds = userIds.Distinct().ToList()
                    };

                    await _notificationService.AddAsync(notificationDto);
                }
            }
        }
        #endregion
    }
}
