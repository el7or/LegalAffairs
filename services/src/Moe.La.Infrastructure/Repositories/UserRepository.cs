using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moe.La.Common;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using Moe.La.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IDistributedCache _cache;
        private const string CacheKey = "Lookup_User_";

        public UserRepository(UserManager<AppUser> userManager,
            LaDbContext context,
            IMapper mapper,
            IUserProvider userProvider,
            IDistributedCache cache)
            : base(context, mapper, userProvider)
        {
            _userManager = userManager;
            _cache = cache;
        }

        public async Task<QueryResultDto<UserListItemDto>> GetAllAsync(UserQueryObject queryObject)
        {
            var result = new QueryResult<AppUser>();

            var query = db.Users
                .Include(m => m.Branch)
                .Include(j => j.JobTitle)
                .Include(m => m.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(m => m.UserRoles)
                  .ThenInclude(s => s.UserRoleDepartmets)
                    .ThenInclude(r => r.Department)
                .Where(u => u.Id != ApplicationConstants.SystemAdministratorId && !u.IsDeleted)
                .AsNoTracking()
                .AsQueryable();

            if (queryObject.WorkItemTypeId.HasValue)
            {
                var workItemype = await db.WorkItemTypes.FirstOrDefaultAsync(w => w.Id == queryObject.WorkItemTypeId);
                if (workItemype != null && workItemype.RolesIds != null)
                    query = query.Where(u => u.UserRoles.Select(ur => ur.RoleId).Any(r => workItemype.RolesIds.Contains(r.ToString())));
            }

            if (queryObject.Enabled)
            {
                query = query.Where(p => p.Enabled == queryObject.Enabled);
            }

            if (queryObject.BranchId != null)
            {
                query = query.Where(p => p.BranchId == queryObject.BranchId);
            }

            if (queryObject.DepartmetId != null)
            {
                query = query.Where(p => p.UserRoles.Any(r => r.UserRoleDepartmets.Any(d => d.DepartmentId == queryObject.DepartmetId)));

            }

            if (!string.IsNullOrEmpty(queryObject.Roles))
            {
                var roles = queryObject.Roles.Split(new char[] { ',' });
                query = query.Where(c => c.UserRoles.Any(cc => roles.Contains(cc.Role.Name)));
                //query = query.Where(l => l.UserRoles.Where(c => roles.Contains(c.Role.Name)).Any());
            }

            //Email
            if (!string.IsNullOrEmpty(queryObject.Email))
                query = query.Where(c => c.Email.Contains(queryObject.Email));

            //FullName
            if (!string.IsNullOrEmpty(queryObject.FullName))
                query = query.Where(c => (c.FirstName + " " + c.LastName).Contains(queryObject.FullName));

            //IdentityNumber
            if (!string.IsNullOrEmpty(queryObject.IdentityNumber))
                query = query.Where(c => c.IdentityNumber.Contains(queryObject.IdentityNumber));


            if (!string.IsNullOrEmpty(queryObject.SearchText))
            {
                query = query.Where(c => c.FirstName.Contains(queryObject.SearchText)
                                      || c.LastName.Contains(queryObject.SearchText)
                                      || c.IdentityNumber.Contains(queryObject.SearchText)
                                      || c.Email.Contains(queryObject.SearchText)
                                      || c.CreatedOn.Date.ToString().Contains(queryObject.SearchText)
                                      || DateTimeHelper.GetHigriDate(c.CreatedOn).Contains(queryObject.SearchText)
                                      || c.IdentityNumber.ToString().Contains(queryObject.SearchText));

            }

            if (queryObject.BranchId.HasValue)
            {
                query = query.Where(p => p.BranchId == queryObject.BranchId);
            }
            if (queryObject.HasConfidentialPermission == true)
            {
                query = query.Where(p => p.UserClaims.Any(c => c.ClaimValue == ApplicationRoleClaimsConstants.ConfidentialMoamla.ClaimValue));
            }

            if (String.IsNullOrEmpty(queryObject.SortBy))
            {
                query = query.OrderBy(v => v.Email);
            }
            else
            {
                var columnsMap = new Dictionary<string, Expression<Func<AppUser, object>>>()
                {
                    ["name"] = v => v.FirstName,
                    ["userName"] = v => v.UserName,
                    //["email"] = v => v.Email,
                    //["createdOn"] = v => v.CreatedOn,
                    //["phoneNumber"] = v => v.PhoneNumber,
                    ["enabled"] = v => v.Enabled,
                    ["roleGroup"] = v => v.UserRoles.FirstOrDefault().Role.NameAr,
                    ["identityNumber"] = v => v.IdentityNumber,
                    ["branch"] = v => v.Branch.Name,
                };

                query = query.ApplySorting(queryObject, columnsMap);
            }


            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<AppUser>, QueryResultDto<UserListItemDto>>(result);
        }

        public async Task<UserDetailsDto> GetDepartmentManagerAsync(int departmentId)
        {
            var user = await db.Users
                .Include(m => m.UserRoles)
                    .ThenInclude(m => m.Role)
                .Include(m => m.UserRoles)
                    .ThenInclude(mm => mm.UserRoleDepartmets)
                        .ThenInclude(mm => mm.Department)
                .FirstOrDefaultAsync(u => u.UserRoles.Any(r => r.UserRoleDepartmets.Any(s => s.DepartmentId == departmentId && s.RoleId == ApplicationRolesConstants.DepartmentManager.Code)));

            return mapper.Map<UserDetailsDto>(user);
        }

        public async Task<List<UserDetailsDto>> GetUsersByRolesAsync(Guid roleId, int? branchId, int? departmetId = null)
        {
            //var rolesList = roles.Split(new char[] { ',' });

            var users = db.Users
                .Include(m => m.UserRoles)
                  .ThenInclude(mm => mm.UserRoleDepartmets)
                .Include(m => m.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(m => m.UserRoles)
                  .ThenInclude(mm => mm.UserRoleDepartmets)
                    .ThenInclude(ur => ur.Department)
                .Where(u => u.UserRoles.Any(cc => cc.RoleId == roleId))
                //.Where(u => u.UserRoles.Any(cc => rolesList.Contains(cc.Role.Id.ToString())))
                .AsNoTracking()
                .AsQueryable();

            if (branchId.HasValue)
            {
                users = users
               .Where(u => u.BranchId == branchId);
            }

            if (departmetId.HasValue)
            {
                users = users
                .Where(u => u.UserRoles
                .Any(r => r.UserRoleDepartmets.Any(s => s.DepartmentId == departmetId)));
            }

            var result = await users.ToListAsync();
            return mapper.Map<List<UserDetailsDto>>(result);
        }

        private async Task<AppUser> UpdateUserCache(Guid id)
        {
            var user = await db.Users
                    .Include(m => m.Branch)
                    .Include(j => j.JobTitle)
                    .Include(m => m.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .Include(m => m.UserRoles)
                        .ThenInclude(ur => ur.UserRoleDepartmets)
                            .ThenInclude(urs => urs.Department)
                    .Include(c => c.UserClaims)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            // Update user data cache value
            await _cache.SetRecordAsync(CacheKey + id.ToString(), user);

            return user;
        }

        public async Task<UserDetailsDto> GetAsync(Guid id)
        {
            // Get user data from cache
            AppUser user = await _cache.GetRecordAsync<AppUser>(CacheKey + id.ToString());

            if (user is null)
            {
                user = await UpdateUserCache(id);
            }

            return mapper.Map<UserDetailsDto>(user);
        }

        public async Task AddAsync(UserDto userDto)
        {
            var entityToAdd = mapper.Map<UserDto, AppUser>(userDto);
            entityToAdd.Id = Guid.NewGuid();
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.CreatedBy = CurrentUser.UserId;

            var result = await _userManager.CreateAsync(entityToAdd, "12345678");

            if (result.Succeeded)
            {
                // if the user created add the roles 
                await _userManager.AddToRolesAsync(entityToAdd, userDto.Roles);

                if (userDto.UserRoleDepartments.Any())
                {
                    var userRolesDepartments = mapper.Map<ICollection<UserRoleDepartmentDto>, ICollection<UserRoleDepartment>>(userDto.UserRoleDepartments);
                    foreach (var item in userRolesDepartments)
                    {
                        item.UserId = entityToAdd.Id;
                        item.CreatedBy = CurrentUser.UserId;
                        item.CreatedOn = DateTime.Now;
                    }
                    db.AppUserRoleDepartmets.AddRange(userRolesDepartments);
                }

                // add the claims from the model
                if (userDto.Claims.Count() > 0)
                    await AddUserClaims(entityToAdd, userDto);
            }

            await UpdateUserCache(entityToAdd.Id);

            mapper.Map(entityToAdd, userDto);
        }

        public async Task EditAsync(UserDto userDto)
        {
            var user = await _userManager.Users
               .Include(m => m.UserRoles)
               .Include(m => m.UserRoles)
                .ThenInclude(ur => ur.UserRoleDepartmets)
               .Include(c => c.UserClaims)
               .FirstOrDefaultAsync(u => u.Id == userDto.Id);

            mapper.Map(userDto, user);

            // update password.
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userDto.Password);
            }

            // Get all user's roles, and remove them

            //map user data
            var userRolesDepartments = mapper.Map<ICollection<UserRoleDepartmentDto>, ICollection<UserRoleDepartment>>(userDto.UserRoleDepartments);
            //get all user role departments from db
            var userRolesDepartmentsInDb = await db.AppUserRoleDepartmets
                .Include(u => u.Department).Where(m => m.UserId == user.Id).ToListAsync();

            //get user role departments from db not in user data to remove it
            var userRolesDepartmentsInDbNotInUserData = userRolesDepartmentsInDb.Except(userRolesDepartments);

            //get user role departments in user data not in db to add it
            var userRolesDepartmentsInUserDataNotInDb = userRolesDepartments.Except(userRolesDepartmentsInDb);

            if (userRolesDepartmentsInDbNotInUserData.Any())
                db.AppUserRoleDepartmets.RemoveRange(userRolesDepartmentsInDbNotInUserData);

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

            // Add the roles from the model
            await _userManager.AddToRolesAsync(user, userDto.Roles);

            //Add user role departments in db
            if (userRolesDepartmentsInUserDataNotInDb.Any())
            {
                foreach (var item in userRolesDepartmentsInUserDataNotInDb)
                {
                    item.CreatedBy = CurrentUser.UserId;
                    item.CreatedOn = DateTime.Now;
                }
                db.AppUserRoleDepartmets.AddRange(userRolesDepartmentsInUserDataNotInDb);

                db.SaveChanges();
            }

            // add the claims from the model
            if (userDto.Claims.Count() > 0)
                await AddUserClaims(user, userDto);

            user.UpdatedBy = CurrentUser.UserId;
            user.UpdatedOn = DateTime.Now;

            await _userManager.UpdateAsync(user);

            await UpdateUserCache(user.Id);

            mapper.Map(user, userDto);
            mapper.Map(userRolesDepartments, userDto.UserRoleDepartments);
        }

        private async Task AddUserClaims(AppUser user, UserDto userDto)
        {
            // get all user's claims, and remove them
            var claims = await _userManager.GetClaimsAsync(user);
            await _userManager.RemoveClaimsAsync(user, claims.ToArray());

            // add Permission claims from the model
            foreach (var claimValue in userDto.Claims)
            {
                // get claim details by claim value
                var claimDetails = await db.RoleClaims.FirstOrDefaultAsync(c => c.ClaimValue == claimValue);

                // get role details by role id
                var roleDetails = await db.Roles.FirstOrDefaultAsync(r => r.Id == claimDetails.RoleId);

                // check user has role to add claim
                var hasRole = await _userManager.IsInRoleAsync(user, roleDetails.Name);

                if (hasRole)
                {
                    var claim = new Claim(claimDetails.ClaimType, claimDetails.ClaimValue);
                    await _userManager.AddClaimAsync(user, claim);
                }
            }

            // add Branch claim
            await _userManager.AddClaimAsync(user, new Claim("Branch", user.BranchId.ToString()));

            // add Department claim
            var departments = userDto.UserRoleDepartments.Select(r => r.DepartmentId).Distinct();

            foreach (var item in departments)
            {
                await _userManager.AddClaimAsync(user, new Claim("Department", item.ToString()));
            }

            await UpdateUserCache(user.Id);
        }

        public async Task RemoveAsync(Guid id)
        {
            var entityToDelete = await db.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            entityToDelete.IsDeleted = true;
            entityToDelete.UpdatedBy = CurrentUser.UserId;
            entityToDelete.UpdatedOn = DateTime.Now;

            // remove departmets
            var userRolesDepartmentsInDb = await db.AppUserRoleDepartmets.Where(m => m.UserId == id).ToListAsync();

            if (userRolesDepartmentsInDb.Any())
            {
                db.AppUserRoleDepartmets.RemoveRange(userRolesDepartmentsInDb);
            }

            // remove refresh token
            var refreshTokensInDb = await db.RefreshTokens.Where(r => r.UserId == id).ToListAsync();

            if (refreshTokensInDb.Any())
            {
                db.RefreshTokens.RemoveRange(refreshTokensInDb);
            }

            // Update user data cache value
            await _cache.SetRecordAsync(CacheKey + entityToDelete.Id.ToString(), entityToDelete);

            await _userManager.UpdateAsync(entityToDelete);

            await UpdateUserCache(entityToDelete.Id);

        }

        public async Task<ICollection<RoleListItemDto>> GetUserRolesAsync(Guid userId)
        {
            AppUser user = await _cache.GetRecordAsync<AppUser>(CacheKey + userId.ToString());

            if (user is null)
            {
                var query = await (from r in db.Roles
                                   join ur in db.UserRoles on r.Id equals ur.RoleId
                                   where ur.UserId == userId
                                   select r).ToListAsync();

                return mapper.Map<ICollection<RoleListItemDto>>(query);
            }
            else
            {
                var appRoles = mapper.Map<ICollection<AppRole>>(user.UserRoles);
                return mapper.Map<ICollection<RoleListItemDto>>(appRoles);
            }
        }

        public async Task<AppUser> GetByUserName(string userName)
        {
            var user = await db.Users
                .Include(m => m.Branch)
                .Include(j => j.JobTitle)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName);

            return mapper.Map<AppUser>(user);
        }

        public async Task<bool> CheckUserExists(string userName)
        {
            var userInDb = await _userManager.FindByNameAsync(userName);
            if (userInDb != null)
                return true;

            return false;
        }

        //public async Task UploadUserPhoto(Guid userId, string FilePath)
        //{
        //    var user = await commandDb.Users.FirstOrDefaultAsync(m => m.Id == userId);
        //    //user.PictureUrl = FilePath;
        //    user.Signature = FilePath;
        //    user.UpdatedBy = userId;
        //    user.UpdatedOn = DateTime.Now;

        //    await _userManager.UpdateAsync(user);

        //    await UpdateUserCache(user.Id);
        //}


        // TODO: Move this method to LegalBoardService
        public async Task<ICollection<ConsultantDto>> GetConsultants(string userName, int? legalBoardId)
        {
            var query = db.Users
                .Where(u => u.UserRoles.Any(r => r.RoleId == ApplicationRolesConstants.LegalConsultant.Code))
                .AsEnumerable();

            if (legalBoardId != null)
            {
                var legalBoard = await db.LegalBoards
                    .Where(l => l.Id == legalBoardId)
                    .Include(l => l.LegalBoardMembers)
                        .ThenInclude(m => m.User)
                    .FirstOrDefaultAsync();

                if (legalBoard != null)
                {
                    query = query.Where(u => legalBoard.LegalBoardMembers.All(m => m.UserId != u.Id));
                }
            }

            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(u => u.FirstName.Contains(userName) || u.LastName.Contains(userName));
            }

            var user = query.ToList();

            return mapper.Map<ICollection<ConsultantDto>>(user);
        }

        public async Task<bool> CheckSameLegalDepartment(Guid researcherId, Guid consultantId)
        {
            var researcher = await db.Users.FindAsync(researcherId);
            var consultant = await db.Users.FindAsync(consultantId);

            return consultant != null && researcher != null && researcher.BranchId == consultant.BranchId;
        }

        public async Task<bool> CheckUserEnabled(Guid userId)
        {
            AppUser user = await _cache.GetRecordAsync<AppUser>(CacheKey + userId.ToString());

            if (user is null)
            {
                var userInDb = await db.Users.FindAsync(userId);
                return userInDb != null && userInDb.Enabled;
            }
            else
            {
                return user.Enabled;
            }
        }

        public async Task<bool> IsInRole(Guid userId, Guid roleId)
        {
            AppUser user = await _cache.GetRecordAsync<AppUser>(CacheKey + userId.ToString());

            if (user is null)
            {
                return await db.UserRoles.AnyAsync(m => m.RoleId == roleId && m.UserId == userId);
            }
            else
            {
                return user.UserRoles.Any(x => x.RoleId == roleId);
            }
        }

        public async Task AssignRoleToUserAsync(Guid userId, string roles)
        {
            var user = await db.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            user.UpdatedBy = userId;
            user.UpdatedOn = DateTime.Now;
            string[] rolesList = roles.Split(',');

            await _userManager.AddToRolesAsync(user, rolesList);
            await _userManager.UpdateAsync(user);

            await UpdateUserCache(user.Id);
        }

        public async Task<UserDto> EnabledUserAsync(Guid userId, bool enabled)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            user.Enabled = enabled;
            user.UpdatedBy = userId;
            user.UpdatedOn = DateTime.Now;

            await _userManager.UpdateAsync(user);

            await UpdateUserCache(user.Id);

            return mapper.Map<UserDto>(user);
        }

        public async Task<ICollection<UserRoleDepartmentDto>> GetUserRolesDepartmentsAsync(Guid userId)
        {
            var query = await db.AppUserRoleDepartmets.
                Include(r => r.UserRole).ThenInclude(r => r.Role)
                .Where(r => r.UserId == userId).ToListAsync();

            return mapper.Map<ICollection<UserRoleDepartmentDto>>(query);
        }

        public async Task<UserDto> GetUserDetailsAsync(Guid userId)
        {
            AppUser user = await _cache.GetRecordAsync<AppUser>(CacheKey + userId.ToString());

            if (user is null)
            {
                var userExist = await _userManager.FindByIdAsync(userId.ToString());

                return mapper.Map<UserDto>(userExist);
            }
            else
            {
                return mapper.Map<UserDto>(user);
            }
        }

        public async Task<bool> CheckJobTitleExists(int jobTitleId)
        {
            var jobTitleExists = await db.Users.Where(u => u.JobTitleId == jobTitleId).CountAsync();
            return jobTitleExists > 0;
        }

        public async Task<bool> IsPhoneNumberExistsAsync(UserDto userDto)
        {
            bool isExists;

            // New
            if (string.IsNullOrEmpty(userDto.Id.ToString()))
            {
                isExists = await db.Users.AnyAsync(c => c.PhoneNumber == userDto.PhoneNumber && !c.IsDeleted);
            }
            else // Update
            {
                isExists = await db.Users.AnyAsync(c => c.Id != userDto.Id && c.PhoneNumber == userDto.PhoneNumber && !c.IsDeleted);
            }

            return isExists;
        }

        #region CheckResearcher

        public async Task<bool> CheckResearcherExists(Guid userId)
        {
            return await db.ResearcherConsultants.Where(u => u.ResearcherId == userId && u.IsActive && !u.IsDeleted).AnyAsync();
        }

        public async Task<bool> CheckCaseResearcherExists(Guid userId)
        {
            return await db.CaseResearchers.Where(u => u.ResearcherId == userId && !u.IsDeleted).AnyAsync();
        }

        public async Task<bool> CheckHearingsResearcherExists(Guid userId)
        {
            return await db.Hearings.Where(u => u.AssignedToId == userId && !u.IsDeleted).AnyAsync();
        }

        #endregion

        #region CheckResearcher

        public async Task<bool> CheckConsultantExists(Guid userId)
        {
            return await db.ResearcherConsultants.Where(u => u.ConsultantId == userId && u.IsActive && !u.IsDeleted).AnyAsync();
        }

        #endregion

        #region CheckLegalBoardMembers

        public async Task<bool> CheckLegalBoardMembersExists(Guid userId)
        {
            return await db.LegalBoardMembers.Where(u => u.UserId == userId && !u.IsDeleted).AnyAsync();
        }
        #endregion

        public async Task<bool> IsMainBoardHeadExist(UserDto userDto)
        {
            bool isExists;
            // if new
            if (string.IsNullOrEmpty(userDto.Id.ToString()))
                isExists = await db.Users
                    .AnyAsync(u => u.UserRoles.Any(r => r.Role.Name == ApplicationRolesConstants.MainBoardHead.Name) && !u.IsDeleted);
            else // update
                isExists = await db.Users
                .AnyAsync(u => u.Id != userDto.Id && u.UserRoles.Any(r => r.Role.Name == ApplicationRolesConstants.MainBoardHead.Name) && !u.IsDeleted);

            return isExists;
        }
    }
}