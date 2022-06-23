using AutoMapper;
using Moe.La.Common;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moe.La.Core.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            #region Domain Models to API Dto:


            CreateMap<AppUser, UserDto>()
                .ForMember(res => res.Roles, opt => opt.MapFrom(u => u.UserRoles.Select(r => r.Role)));


            CreateMap<AppUser, UserListItemDto>()
                 .ForMember(res => res.Signature, opt => opt.MapFrom(u => u.Signature != null ? Encoding.UTF8.GetString(u.Signature, 0, u.Signature.Length) : null))
                .ForMember(res => res.Branch, opt => opt.MapFrom(u => u.Branch.Name))
                .ForMember(res => res.JobTitle, opt => opt.MapFrom(u => u.JobTitle.Name))
                .ForMember(res => res.Roles, opt => opt.MapFrom(u => u.UserRoles.Select(r => r.Role)))
                .ForMember(res => res.RoleGroup, opt => opt.MapFrom(u => string.Join(" - ", u.UserRoles.Select(r => r.Role.NameAr))))
                .ForMember(res => res.CreatedOnHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.CreatedOn)))
                .ForMember(res => res.Departments, opt => opt.Ignore())
                .ForMember(res => res.Researchers, opt => opt.MapFrom(u => u.Researchers
                .Select(r => new KeyValuePairsDto<Guid> { Id = r.ResearcherId, Name = r.Researcher.FirstName + " " + r.Researcher.LastName }).ToList()))
                .AfterMap((appUser, userListItemDto) =>
                {
                    foreach (var item in appUser.UserRoles.Select(m => m.UserRoleDepartmets))
                    {
                        foreach (var department in item.Select(s => s.Department.Name))
                            userListItemDto.Departments.Add(department);
                    }
                    userListItemDto.DepartmentsGroup = string.Join(" - ", userListItemDto.Departments);

                });




            CreateMap<AppUser, UserDetailsDto>()
                 .ForMember(res => res.Signature, opt => opt.MapFrom(u => u.Signature != null ? Encoding.UTF8.GetString(u.Signature, 0, u.Signature.Length) : null))
                .ForMember(res => res.Branch, opt => opt.MapFrom(u => u.Branch))
                .ForMember(res => res.JobTitle, opt => opt.MapFrom(u => u.JobTitle))
                .ForMember(res => res.Roles, opt => opt.MapFrom(u => u.UserRoles.Select(r => r.Role)))
                .ForMember(res => res.Claims, opt => opt.MapFrom(u => u.UserClaims))
                .ForMember(res => res.UserRoleDepartments, opt => opt.Ignore())
                .AfterMap((user, UserDto) =>
                {
                    foreach (var userRoleDepartmets in user.UserRoles)
                    {
                        foreach (var item in userRoleDepartmets.UserRoleDepartmets)
                        {
                            UserDto.UserRoleDepartments.Add(new UserRoleDepartmentDto
                            {
                                Id = item.Id,
                                UserId = item.UserId,
                                RoleId = item.RoleId,
                                RoleName = userRoleDepartmets.Role.Name,
                                RoleNameAr = userRoleDepartmets.Role.NameAr,
                                DepartmentName = item.Department.Name,
                                DepartmentId = item.DepartmentId
                            });
                        }
                    }

                });

            CreateMap<AppUser, ConsultantDto>()
            .ForMember(res => res.ConsultantId, opt => opt.MapFrom(u => u.Id))
            .ForMember(res => res.Name, opt => opt.MapFrom(u => u.FirstName + " " + u.LastName))
            .ForMember(res => res.EmployeeNo, opt => opt.MapFrom(u => u.EmployeeNo));


            CreateMap<AppRole, RoleListItemDto>();

            CreateMap<AppRole, RoleDto>();

            CreateMap<AppUserRole, UserRoleDto>();

            CreateMap<UserRoleDepartment, UserRoleDepartmentListItemDto>()
                .ForMember(res => res.UserId, opt => opt.MapFrom(u => u.UserId))
                .ForMember(res => res.RoleId, opt => opt.MapFrom(u => u.RoleId))
                .ForMember(res => res.Department, opt => opt.MapFrom(u => u.Department));

            CreateMap<UserRoleDepartment, UserRoleDepartmentDto>();

            CreateMap<AppRoleClaim, RoleClaimListItemDto>();

            CreateMap<ResearcherConsultant, ResearcherConsultantDto>();

            CreateMap<ResearcherConsultantDto, ResearcherConsultantListItemDto>()
                .ForMember(res => res.StartDateHigri, opt => opt.MapFrom(c => DateTimeHelper.GetHigriDate(c.StartDate)));

            CreateMap<ResearcherConsultantHistory, ResearcherConsultantHistoryDto>();

            CreateMap<ResearcherConsultantDto, ResearcherConsultantHistoryDto>()
               .ForMember(res => res.ResearcherConsultantId, opt => opt.MapFrom(U => U.Id));

            CreateMap<UserRoleDepartment, UserRoleDepartmentDto>()
                .ForMember(s => s.RoleId, opt => opt.MapFrom(s => s.UserRole.Role.Id))
                .ForMember(s => s.DepartmentId, opt => opt.MapFrom(s => s.DepartmentId))
                .ForMember(s => s.RoleName, opt => opt.MapFrom(s => s.UserRole.Role.Name))
                .ForMember(s => s.RoleNameAr, opt => opt.MapFrom(s => s.UserRole.Role.NameAr))
                .ForMember(s => s.DepartmentName, opt => opt.MapFrom(s => s.Department.Name));

            CreateMap<AppUserRole, AppRole>()
                .ForMember(s => s.Id, opt => opt.MapFrom(s => s.RoleId))
                .ForMember(s => s.IsDistributable, opt => opt.MapFrom(s => s.Role.IsDistributable))
                .ForMember(s => s.Name, opt => opt.MapFrom(s => s.Role.Name))
                .ForMember(s => s.NameAr, opt => opt.MapFrom(s => s.Role.NameAr))
                .ForMember(s => s.NormalizedName, opt => opt.MapFrom(s => s.Role.NormalizedName))
                .ForMember(s => s.Priority, opt => opt.MapFrom(s => s.Role.Priority));

            #endregion

            #region API Dto to Domain:

            CreateMap<RoleDto, AppRole>()
                .ForMember(r => r.Id, opt => opt.Ignore());

            CreateMap<UserDto, AppUser>()
                .ForMember(c => c.Id, opt => opt.Ignore())
                .ForMember(c => c.Signature, opt => opt.MapFrom(U => Encoding.ASCII.GetBytes(U.Signature)))
                //.ForMember(m => m.UserName, map => map.MapFrom(vm => vm.Email))
                .ForMember(m => m.Researchers, opt => opt.Ignore())
                .AfterMap((userRes, user) =>
                {
                    var removedResearcher = new List<ResearcherConsultant>();
                    foreach (var researcher in user.Researchers)
                        if (!userRes.Researchers.Contains(researcher.Researcher.Id))
                            removedResearcher.Add(researcher);

                    foreach (var item in removedResearcher)
                        user.Researchers.Remove(item);

                    foreach (var id in userRes.Researchers)
                        if (!user.Researchers.Select(c => c.ResearcherId).Contains(id))
                            user.Researchers.Add(new ResearcherConsultant
                            {
                                ResearcherId = id,
                                ConsultantId = user.Id
                            });

                    // update branch value in claims
                    var branchClaimToUpdate = user.UserClaims.Where(c => c.ClaimType == "Branch").FirstOrDefault();
                    if (branchClaimToUpdate != null)
                    {
                        branchClaimToUpdate.ClaimValue = userRes.BranchId.ToString();
                    }

                });

            // CreateMap<UserQueryObjectDto, UserQueryObject>();
            CreateMap<ResearcherConsultantDto, ResearcherConsultant>();

            CreateMap<ResearcherConsultantHistoryDto, ResearcherConsultantHistory>()
                .ForMember(r => r.Id, opt => opt.Ignore());
            CreateMap<ResearcherConsultant, ResearcherConsultantHistory>()
                .ForMember(res => res.ResearcherConsultantId, opt => opt.MapFrom(U => U.Id))
                .ForMember(r => r.Id, opt => opt.Ignore());

            CreateMap<UserRoleDepartmentDto, UserRoleDepartment>()
                .ForMember(s => s.Id, opt => opt.Ignore());
            #endregion

            #region Refresh token
            CreateMap<RefreshTokenDto, RefreshToken>()
                .ForMember(res => res.Id, opt => opt.Ignore());
            #endregion

        }
    }
}
