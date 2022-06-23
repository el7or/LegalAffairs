using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IUserService : IUserProvider
    {
        Task<ReturnResult<QueryResultDto<UserListItemDto>>> GetAllAsync(UserQueryObject queryObject);

        Task<ReturnResult<UserDetailsDto>> GetAsync(Guid id);

        Task<ReturnResult<UserDto>> AddAsync(UserDto model);

        Task<ReturnResult<UserDto>> EditAsync(UserDto model);

        Task<ReturnResult<bool>> RemoveAsync(Guid id);

        Task<ReturnResult<ICollection<RoleListItemDto>>> GetUserRolesAsync(Guid userId);

        Task<ReturnResult<AppUser>> GetByUserName(string userName);

        //Task<ReturnResult<string>> AddUserPhotoAsync(IFormFile file, UserDetailsDto user = null);

        Task<ReturnResult<ICollection<ConsultantDto>>> GetConsultants(string userName, int? legalBoardId);

        //Task<ReturnResult<UserDto>> GetUserCachValueAsync(Guid userId);

        //Task<ReturnResult<UserDto>> SetUserCachValueAsync(UserDto userDto);

        Task<ReturnResult<bool>> EnabledUserAsync(Guid userId, bool enabled);

        Task<ReturnResult<ICollection<UserRoleDepartmentDto>>> GetUserRolesDepartmentsAsync(UserDto user);

        Task<ReturnResult<ICollection<UserRoleDepartmentDto>>> GetUserRolesDepartmentsAsync(Guid userId);

        Task<ReturnResult<UserDetailsDto>> GetDepartmentManagerAsync(int departmentId);

        Task<ReturnResult<List<UserDetailsDto>>> GetUsersByRolesAsync(Guid roleId, int? branchId, int? departmetId = null);

        Task<ReturnResult<bool>> AssignRoleToUserAsync(Guid userId, string roles);

        Task<ReturnResult<bool>> IsInRole(Guid userId, Guid roleId);

        Task<ReturnResult<bool>> CheckUserEnabled(Guid userId);

        Task<ReturnResult<bool>> CheckSameLegalDepartment(Guid researcherId, Guid consultantId);

        Task<ReturnResult<bool>> CheckJobTitleExists(int jobTitleId);

    }

}
