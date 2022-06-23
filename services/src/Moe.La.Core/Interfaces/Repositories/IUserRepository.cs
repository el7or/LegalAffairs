using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<QueryResultDto<UserListItemDto>> GetAllAsync(UserQueryObject queryObject);

        Task<UserDetailsDto> GetAsync(Guid id);

        Task<UserDetailsDto> GetDepartmentManagerAsync(int departmentId);

        Task<List<UserDetailsDto>> GetUsersByRolesAsync(Guid roleId, int? branchId, int? departmetId = null);

        Task AddAsync(UserDto userDto);

        Task EditAsync(UserDto userDto);

        Task RemoveAsync(Guid id);

        Task<ICollection<RoleListItemDto>> GetUserRolesAsync(Guid userId);

        Task<AppUser> GetByUserName(string userName);

        Task<bool> CheckUserExists(string userName);

        //Task UploadUserPhoto(Guid userId, string FilePath);

        Task<ICollection<ConsultantDto>> GetConsultants(string userName, int? legalBoardId);

        Task<bool> CheckSameLegalDepartment(Guid researcherId, Guid consultantId);

        Task<bool> CheckUserEnabled(Guid userId);

        /// <summary>
        /// Check if the user is in a given role.
        /// </summary>
        /// <param name="userId">user ID.</param>
        /// <param name="roleId">role ID</param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> IsInRole(Guid userId, Guid roleId);

        Task AssignRoleToUserAsync(Guid userId, string roles);

        Task<UserDto> EnabledUserAsync(Guid userId, bool enabled);

        Task<ICollection<UserRoleDepartmentDto>> GetUserRolesDepartmentsAsync(Guid userId);

        Task<UserDto> GetUserDetailsAsync(Guid userId);

        Task<bool> CheckJobTitleExists(int jobTitleId);

        Task<bool> IsPhoneNumberExistsAsync(UserDto userDto);

        Task<bool> CheckResearcherExists(Guid userId);

        Task<bool> CheckCaseResearcherExists(Guid userId);

        Task<bool> CheckHearingsResearcherExists(Guid userId);

        Task<bool> CheckConsultantExists(Guid userId);

        Task<bool> CheckLegalBoardMembersExists(Guid userId);

        Task<bool> IsMainBoardHeadExist(UserDto userDto);
    }
}