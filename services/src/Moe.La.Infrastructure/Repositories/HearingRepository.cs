using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Common;
using Moe.La.Core.Constants;
using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Infrastructure.DbContexts;
using Moe.La.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Moe.La.Infrastructure.Repositories
{
    public class HearingRepository : RepositoryBase, IHearingRepository
    {
        public HearingRepository(LaDbContext context, IMapper mapper, IUserProvider userProvider)
            : base(context, mapper, userProvider)
        {
        }

        public async Task<QueryResultDto<HearingListItemDto>> GetAllAsync(HearingQueryObject queryObject)
        {
            var result = new QueryResult<Hearing>();

            var query = db.Hearings
                .Include(c => c.Case)
                 .ThenInclude(t => t.Researchers)
                  .ThenInclude(r => r.Researcher)
                .Include(c => c.AssignedTo)
                .Include(c => c.Court)
                .Include(c => c.CaseSupportingDocumentRequests)
                 .ThenInclude(d => d.Request)
                .AsNoTracking()
                .AsQueryable();

            IQueryable<Hearing> finalQuery = null;

            #region fiter hearings by roles
             
            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.LegalResearcher.Name))
            {
                var newQuery = query.Where(c => c.Case.Researchers.Any(r => r.ResearcherId == CurrentUser.UserId) || c.AssignedToId == CurrentUser.UserId);
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);
            }

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.LegalConsultant.Name))
            {
                var newQuery = query.Where(c => c.Case.Researchers.Any(r => r.Researcher.Consultants.Any(c => c.ConsultantId == CurrentUser.UserId && c.IsActive == true)));
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);
            }

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.RegionsSupervisor.Name))
            {
                var newQuery = query.Where(c => c.Case.Status == CaseStatuses.SentToRegionsSupervisor || c.Case.Status == CaseStatuses.ReturnedToRegionsSupervisor || c.Case.Status == CaseStatuses.ReceivedByRegionsSupervisor);
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);
            }

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.BranchManager.Name))
            {
                var newQuery = query.Where(c => c.Case.Status == CaseStatuses.SentToBranchManager || c.Case.Status == CaseStatuses.ReceivedByBranchManager && c.Case.BranchId == CurrentUser.BranchId);
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);
            }

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.DepartmentManager.Name))
            {
                // check if user is litigationmanager
                var isLitigationManager = await db.AppUserRoleDepartmets.Where(u => u.UserId == CurrentUser.UserId
                && u.RoleId == ApplicationRolesConstants.DepartmentManager.Code
                && u.DepartmentId == 1).AnyAsync();

                if (isLitigationManager)
                {
                    var newQuery = query.Where(c => c.Case.BranchId == CurrentUser.BranchId && isLitigationManager);
                    if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);
                }
            }

            #endregion
            // if final query equal null
            if (finalQuery == null)
                finalQuery = query.Where(x => false);


            if (queryObject.CaseId.HasValue)
                finalQuery = finalQuery.Where(s => s.CaseId == queryObject.CaseId);

            if (queryObject.CourtId.HasValue)
                finalQuery = finalQuery.Where(s => s.CourtId == queryObject.CourtId);

            if (queryObject.Status.HasValue)
                finalQuery = finalQuery.Where(s => (int)s.Status == queryObject.Status);

            if (!string.IsNullOrEmpty(queryObject.From))
            {
                var from = DateTimeHelper.FromHijriToGoergian(queryObject.From);
                finalQuery = finalQuery.Where(p => p.HearingDate.Date >= from.Date);
            }

            if (!string.IsNullOrEmpty(queryObject.To))
            {
                var to = DateTimeHelper.FromHijriToGoergian(queryObject.From);
                finalQuery = finalQuery.Where(p => p.HearingDate.Date <= to.Date);
            }
            if (queryObject.HearingNumber.HasValue)
                finalQuery = finalQuery.Where(s => s.HearingNumber == queryObject.HearingNumber);

            if (!string.IsNullOrEmpty(queryObject.HearingDate))
            {
                DateTime hearingDate = Convert.ToDateTime(queryObject.HearingDate);
                finalQuery = finalQuery.Where(p => p.HearingDate.Date == hearingDate.Date);
            }

            if (!string.IsNullOrEmpty(queryObject.From))
            {
                DateTime from = Convert.ToDateTime(queryObject.From);
                finalQuery = finalQuery.Where(p => p.HearingDate.Date >= from.Date);
            }

            if (!string.IsNullOrEmpty(queryObject.To))
            {
                DateTime to = Convert.ToDateTime(queryObject.To);
                finalQuery = finalQuery.Where(p => p.HearingDate.Date <= to.Date);
            }
            if (!string.IsNullOrEmpty(queryObject.SearchText))
            {
                finalQuery = finalQuery.Where(m =>
                m.Case.Subject.Contains(queryObject.SearchText) ||
                m.Court.Name.Contains(queryObject.SearchText) ||
                m.CaseId.ToString().Contains(queryObject.SearchText)
                );
            }
            var columnsMap = new Dictionary<string, Expression<Func<Hearing, object>>>()
            {
                ["subject"] = v => v.Case.Subject,
                ["hearingNumber"] = v => v.HearingNumber,
                ["hearingDate"] = v => v.HearingDate,
                ["court"] = v => v.Court.Name
            };

            finalQuery = finalQuery.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await finalQuery.CountAsync();

            finalQuery = finalQuery.ApplyPaging(queryObject);

            result.Items = await finalQuery.AsNoTracking().ToListAsync();

            return mapper.Map<QueryResult<Hearing>, QueryResultDto<HearingListItemDto>>(result);
        }

        public async Task<HearingDetailsDto> GetAsync(int id)
        {
            var hearing = await db.Hearings
                .Include(c => c.HearingLegalMemoReviewRequests)
                 .ThenInclude(c => c.Request)
                    .ThenInclude(cc => cc.UpdatedByUser)
                .Include(c => c.HearingLegalMemoReviewRequests)
                 .ThenInclude(cc => cc.LegalMemo)
                  .ThenInclude(ccc => ccc.UpdatedByUser)
                .Include(c => c.Case)
                 .ThenInclude(c => c.CaseTransactions)
                  .ThenInclude(u => u.CreatedByUser)
                    .ThenInclude(ur => ur.UserRoles)
                      .ThenInclude(r => r.Role)
                .Include(c => c.Case)
                    .ThenInclude(m => m.Court)
                .Include(c => c.Case)
                    .ThenInclude(m => m.CaseRule)
                .Include(c => c.Case)
                    .ThenInclude(m => m.SecondSubCategory)
                        .ThenInclude(mm => mm.FirstSubCategory)
                        .ThenInclude(mm => mm.MainCategory)
                 .Include(c => c.Case)
                    .ThenInclude(m => m.Parties)
                 .Include(c => c.Case)
                    .ThenInclude(m => m.Parties)
                    .ThenInclude(p => p.Party)
                        .ThenInclude(mm => mm.IdentityType)
                 .Include(c => c.Case)
                    .ThenInclude(m => m.Parties)
                    .ThenInclude(p => p.Party)
                        .ThenInclude(mm => mm.IdentityType)
                        .Include(c => c.Case)
                    .ThenInclude(m => m.CaseGrounds)
                        .Include(c => c.Case)
                    .ThenInclude(m => m.Attachments)
                    .ThenInclude(m => m.Attachment)
                    .ThenInclude(m => m.AttachmentType)
               .Include(c => c.Case)
                .ThenInclude(m => m.Attachments)
                   .ThenInclude(m => m.Attachment)
                .Include(c => c.AssignedTo)
                .Include(c => c.Court)
                .Include(c => c.HearingLegalMemos)
                 .ThenInclude(cc => cc.LegalMemo)
                .Include(c => c.CaseSupportingDocumentRequests)
                  .ThenInclude(r => r.Request)
                    .ThenInclude(r => r.CreatedByUser)
                .Include(m => m.Attachments)
                   .ThenInclude(m => m.Attachment)
                .Include(m => m.Attachments)
                   .ThenInclude(m => m.Attachment)
                   .ThenInclude(m => m.AttachmentType)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            var _hearingDetailsDto = mapper.Map<HearingDetailsDto>(hearing);

            if (_hearingDetailsDto.Status.Id == (int)HearingStatuses.Closed && hearing.Case.ReceivingJudgmentDate != null && !_hearingDetailsDto.IsPronouncedJudgment.Value)
            {
                _hearingDetailsDto.IsEditable = false;

            }
            if (db.Hearings.Where(h => h.CaseId == hearing.CaseId && h.HearingDate >= hearing.HearingDate && h.Id != hearing.Id).Count() > 0)
            {
                _hearingDetailsDto.IsHasNextHearing = true;
            }

            return _hearingDetailsDto;
        }

        public async Task AddAsync(HearingDto hearingDto)
        {
            var entityToAdd = mapper.Map<Hearing>(hearingDto);

            //var userIsResearcher = CurrentUser.Roles.Contains(ApplicationRolesConstants.LegalResearcher.Name);
            //if (userIsResearcher)
            //{
            //    entityToAdd.AssignedToId = CurrentUser.UserId;
            //}

            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.CreatedBy = CurrentUser.UserId;

            await db.Hearings.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // get hearing id to use it in attachment
            hearingDto.Id = entityToAdd.Id;
        }

        public async Task EditAsync(HearingDto hearingDto)
        {
            var entityToUpdate = await db.Hearings.Include(h => h.Attachments).Where(h => h.Id == hearingDto.Id).FirstOrDefaultAsync();

            mapper.Map(hearingDto, entityToUpdate);

            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var entityToDelete = await db.Hearings.FindAsync(id);

            entityToDelete.UpdatedBy = CurrentUser.UserId;
            entityToDelete.UpdatedOn = DateTime.Now;
            entityToDelete.IsDeleted = true;

            await db.SaveChangesAsync();
        }

        public async Task ReceivingJudgmentAsync(ReceivingJudgmentDto receivingJudgmentDto)
        {
            var hearing = await db.Hearings.FindAsync(receivingJudgmentDto.HearingId);
            hearing.IsPronouncedJudgment = receivingJudgmentDto.IsPronouncedJudgment;
            hearing.UpdatedBy = CurrentUser.UserId;
            hearing.UpdatedOn = DateTime.Now;
            await db.SaveChangesAsync();
            ////
            var entityToUpdate = await db.Cases.FindAsync(receivingJudgmentDto.CaseId);
            entityToUpdate.PronouncingJudgmentDate = receivingJudgmentDto.PronouncingJudgmentDate;
            entityToUpdate.ReceivingJudgmentDate = receivingJudgmentDto.ReceivingJudgmentDate;
            //entityToUpdate.ObjectionJudgmentLimitDate = receivingJudgmentDto.ReceivingJudgmentDate?.AddDays(30);
            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();
        }

        public async Task RemoveReceivingJudgmentDataAsync(int hearingId)
        {
            var hearing = await db.Hearings.FindAsync(hearingId);
            hearing.IsPronouncedJudgment = false;
            hearing.UpdatedBy = CurrentUser.UserId;
            hearing.UpdatedOn = DateTime.Now;
            await db.SaveChangesAsync();
            ////
            var entityToUpdate = await db.Cases.FindAsync(hearing.CaseId);
            entityToUpdate.PronouncingJudgmentDate = null;
            entityToUpdate.ReceivingJudgmentDate = null;
            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();
        }

        public async Task<int> GetMaxHearingNumberAsync(int caseId)
        {
            var hearing = await db.Hearings
                .Where(h => h.CaseId == caseId && !h.IsDeleted)
                .OrderByDescending(h => h.HearingNumber)
                .FirstOrDefaultAsync();

            if (hearing != null)
                return hearing.HearingNumber ?? 0;
            else
                return 0;
        }

        public async Task<int> GetFirstHearingIdAsync(int caseId)
        {
            var hearing = await db.Hearings
                .Where(h => h.CaseId == caseId && !h.IsDeleted)
                .OrderByDescending(h => h.Id)
                .FirstOrDefaultAsync();

            if (hearing != null)
                return hearing.Id;
            else
                return 0;
        }

        public async Task<bool> IsHearingNumberExistsAsync(HearingNumberDto hearing)
        {
            return await db.Hearings
            .AnyAsync(h => h.Id != hearing.Id && h.CaseId == hearing.CaseId && h.HearingNumber == hearing.HearingNumber && !h.IsDeleted);
        }

        public async Task<bool> IsHearingDateExistsAsync(HearingDto hearing)
        {
            // if update hearing
            if (hearing.Id > 0)
                return await db.Hearings
                    .AnyAsync(h => h.Id != hearing.Id && h.CaseId == hearing.CaseId && h.HearingDate.Date == hearing.HearingDate.Date && !h.IsDeleted);
            else // if new hearing
                return await db.Hearings
               .AnyAsync(h => h.CaseId == hearing.CaseId && h.HearingDate.Date == hearing.HearingDate.Date && !h.IsDeleted);
        }

        public async Task<bool> CheckPleadingHearingsDate(HearingDto hearing)
        {
            // if update hearing
            if (hearing.Id > 0)
                return await db.Hearings
                    .AnyAsync(h => h.Id != hearing.Id && h.CaseId == hearing.CaseId && h.HearingDate.Date > hearing.HearingDate.Date && !h.IsDeleted);
            else // if new hearing
                return await db.Hearings
               .AnyAsync(h => h.CaseId == hearing.CaseId && h.HearingDate.Date > hearing.HearingDate.Date && !h.IsDeleted);
        }

        public async Task<bool> IsFirstHearingAsync(HearingDto hearing)
        {
            bool isFirstHearing = false;

            var _case = await db.Cases
                .Include(m => m.Hearings)
                .FirstOrDefaultAsync(c => c.Id == hearing.CaseId);

            //first hearing
            if (_case.Hearings.Count == 0)
            {
                isFirstHearing = true;
            }

            return isFirstHearing;
        }

        public async Task<bool> IsReceivedJudgmentCase(int caseId)
        {
            var _case = await db.Cases
                .FirstOrDefaultAsync(c => c.Id == caseId);

            return _case.ReceivingJudgmentDate != null;
        }

        public async Task<Hearing> GetCasePronouncingJudgmentHearing(int hearingId, int caseId)
        {
            // if update hearing
            if (hearingId > 0)
                return await db.Hearings.Include(x => x.Case).FirstOrDefaultAsync(h => h.Id != hearingId && h.CaseId == caseId && h.Type == HearingTypes.PronouncingJudgment && !h.IsDeleted);
            else  // if new hearing
                return await db.Hearings.Include(x => x.Case).FirstOrDefaultAsync(h => h.CaseId == caseId && h.Type == HearingTypes.PronouncingJudgment && !h.IsDeleted);
        }

        public async Task<bool> IsCaseSchedulingHearingExists(HearingDto hearing)
        {
            // if update hearing
            if (hearing.Id > 0)
                return await db.Hearings
                    .AnyAsync(h => h.Id != hearing.Id && h.CaseId == hearing.CaseId && h.Status == HearingStatuses.Scheduled && !h.IsDeleted);
            else // if new hearing
                return await db.Hearings
               .AnyAsync(h => h.CaseId == hearing.CaseId && h.Status == HearingStatuses.Scheduled && !h.IsDeleted);
        }

        public async Task<bool> CheckUserAsync(HearingDto hearing)
        {
            bool isAllowed = false;

            var isCurrentUserResearcher = db.Users.Include(u => u.UserRoles)
           .Where(u => u.Id == CurrentUser.UserId && u.UserRoles.Any(m => m.RoleId == ApplicationRolesConstants.LegalResearcher.Code)).Any();

            if (isCurrentUserResearcher)
                isAllowed = await db.CaseResearchers.AnyAsync(m => m.CaseId == hearing.CaseId && m.ResearcherId == CurrentUser.UserId);


            return isAllowed;
        }

        public async Task<QueryResultDto<HearingListItemDto>> GetUpcomingHearingsAsync(int days)
        {
            DateTime selectedDate = DateTime.Now.AddDays(days);

            var hearings = await GetAllAsync(new HearingQueryObject
            {
                From = selectedDate.ToShortDateString(),
                To = selectedDate.ToShortDateString()
            });

            return hearings;
        }

        public async Task<QueryResultDto<HearingListItemDto>> GetUnclosedHearingsAsync(int days)
        {
            DateTime selectedDate = DateTime.Now.AddDays(-1 * days);

            var hearings = await GetAllAsync(new HearingQueryObject
            {
                From = selectedDate.ToShortDateString(),
                To = selectedDate.ToShortDateString(),
                Closed = false
            });

            return hearings;
        }

        public async Task<KeyValuePairsDto<Guid>> AssignHearingToAsync(int hearingId, Guid attendantId)
        {
            var hearing = await db.Hearings.FindAsync(hearingId);

            hearing.AssignedToId = attendantId;
            hearing.UpdatedBy = CurrentUser.UserId;
            hearing.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();

            var hearingDto = await GetAsync(hearing.Id);

            return hearingDto.AssignedTo;
        }

        public Task FinishHearing()
        {
            var unFinishedHearings = db.Hearings
               .Where(h => h.Status == HearingStatuses.Scheduled && h.HearingDate < DateTime.Now)
               .ToList();

            foreach (var hearing in unFinishedHearings)
            {
                hearing.UpdatedBy = CurrentUser.UserId;
                hearing.UpdatedOn = DateTime.Now;
                hearing.Status = HearingStatuses.Finished;

            }
            db.SaveChanges();

            return Task.CompletedTask;
        }

        public Task CloseHearing()
        {
            var hearings = db.Hearings
               .Where(m => m.Status == HearingStatuses.Finished)
               .OrderBy(m => m.CaseId)
               .ThenByDescending(m => m.HearingDate)
               .ThenByDescending(m => m.HearingTime)
               .ToList();

            var caseIds = hearings.Select(m => m.CaseId).Distinct();

            foreach (var caseId in caseIds)
            {
                var caseHearings = hearings.Where(m => m.CaseId == caseId).ToList();

                if (caseHearings.Count() > 1)
                {
                    for (int i = 1; i < caseHearings.Count(); i++)
                    {
                        caseHearings[i].Status = HearingStatuses.Closed;
                        caseHearings[i].UpdatedBy = CurrentUser.UserId;
                        caseHearings[i].UpdatedOn = DateTime.Now;
                    }
                }
            }

            db.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<List<Hearing>> GetHearingOutOfDate()
        {
            var FinishedHearings = db.Hearings
                .Include(u => u.AssignedTo)
                .Where(h => h.Status != HearingStatuses.Closed
                    && h.HearingDate.Date == DateTime.Now.Date.AddDays(-2)
                    && h.AssignedToId != null)
                .ToListAsync();
            return FinishedHearings;
        }

        public async Task<List<ResearcherHearingDto>> GetHearingsOrderedByResearcher()
        {
            var hearings = await db.Hearings.Include(h => h.AssignedTo).Where(h => h.AssignedToId != null).ToListAsync();
            var researcherHearings = hearings.GroupBy(p => p.AssignedToId.Value,
                                    (key, g) => new ResearcherHearingDto { ResearcherId = key, Hearings = g })
                                    .Where(uh => uh.Hearings.Count() >= 2
                                                                    && uh.Hearings.Any(h => h.Status == HearingStatuses.Finished
                                                                    && h.HearingDate.Month == DateTime.Now.Month)).ToList();
            return researcherHearings;
        }

        public async Task<List<Hearing>> ApproachHearingByResearcher()
        {
            var approachResearcherHearings = await db.Hearings
                    .Include(h => h.Court)
                    .Where(h => h.AssignedToId != null && (h.HearingDate.Date == DateTime.Now.AddDays(2).Date || h.HearingDate.Date == DateTime.Now.AddDays(3).Date)).ToListAsync();

            return approachResearcherHearings;
        }

        public async Task<QueryResultDto<UserListItemDto>> GetConsultantsAndResearchers(UserQueryObject queryObject)
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
                    ["email"] = v => v.Email,
                    ["createdOn"] = v => v.CreatedOn,
                    ["phoneNumber"] = v => v.PhoneNumber,
                    ["enabled"] = v => v.Enabled,
                    ["roleGroup"] = v => v.UserRoles.FirstOrDefault().Role.NameAr,
                };

                query = query.ApplySorting(queryObject, columnsMap);
            }


            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.ToListAsync();

            return mapper.Map<QueryResult<AppUser>, QueryResultDto<UserListItemDto>>(result);
        }

        public async Task UpdateHearingsCourtsAsync(int caseId, int newCourtId)
        {
            var hearings = await db.Hearings.Where(c => c.CaseId == caseId).ToListAsync();

            foreach (var hearing in hearings)
            {
                var entityToUpdate = await db.Hearings.FindAsync(hearing.Id);

                entityToUpdate.CourtId = newCourtId;
                entityToUpdate.UpdatedBy = CurrentUser.UserId;
                entityToUpdate.UpdatedOn = DateTime.Now;
            }

            await db.SaveChangesAsync();
        }
    }
}
