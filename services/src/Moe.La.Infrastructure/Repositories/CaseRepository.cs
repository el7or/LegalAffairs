using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class CaseRepository : RepositoryBase, ICaseRepository
    {
        public CaseRepository(LaDbContext db, IMapper mapperConfig, IUserProvider userProvider)
            : base(db, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<CaseListItemDto>> GetAllAsync(CaseQueryObject queryObject)
        {
            var result = new QueryResult<Case>();
            var query = db.Cases
                .Include(c => c.CaseRule)
                .Include(m => m.Parties)
                 .ThenInclude(m => m.Party)
                .Include(c => c.Court)
                .Include(r => r.RelatedCase)
                .Include(t => t.Researchers)
                 .ThenInclude(r => r.Researcher)
                  .ThenInclude(r => r.Consultants)
                .Include(c => c.Hearings)
                .Include(g => g.CaseGrounds)
                .Include(c => c.SecondSubCategory)
                 .ThenInclude(cc => cc.FirstSubCategory)
                  .ThenInclude(cc => cc.MainCategory)
                .Include(c => c.Branch)
                .AsQueryable();

            IQueryable<Case> finalQuery = null;

            //return cases list based on role
            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.DataEntry.Name))
            {
                IQueryable<Case> newQuery = null;

                if (queryObject.IsForChooseRelatedCase == true)
                    newQuery = query.IgnoreQueryFilters().Where(c => c.BranchId == CurrentUser.BranchId && (c.Status == CaseStatuses.DoneJudgment || c.Status == CaseStatuses.ClosedCase || c.Status == CaseStatuses.ObjectionRecorded));
                else
                    newQuery = query.IgnoreQueryFilters().Where(c => c.CreatedBy == CurrentUser.UserId && (c.Status == CaseStatuses.Draft || c.Status == CaseStatuses.Deleted));
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);
            }

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.LegalResearcher.Name))
            {
                IQueryable<Case> newQuery = null;

                if (queryObject.IsForChooseRelatedCase == true)
                    newQuery = query.Where(c => c.BranchId == CurrentUser.BranchId && (c.Status == CaseStatuses.DoneJudgment || c.Status == CaseStatuses.ClosedCase || c.Status == CaseStatuses.ObjectionRecorded));
                else
                    newQuery = query.Where(c => c.Researchers.Any(r => r.ResearcherId == CurrentUser.UserId) && c.Status != CaseStatuses.Draft);
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);
            }

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.LegalConsultant.Name))
            {
                // get consultant researchers cases
                var newQuery = query.Where(c => c.Researchers.Any(r => r.Researcher.Consultants.Any(c => c.ConsultantId == CurrentUser.UserId && c.IsActive == true)) && c.Status != CaseStatuses.Draft);
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);
            }

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.RegionsSupervisor.Name))
            {
                var newQuery = query.Where(c => c.Status == CaseStatuses.SentToRegionsSupervisor || c.Status == CaseStatuses.ReturnedToRegionsSupervisor || c.Status == CaseStatuses.ReceivedByRegionsSupervisor || c.Status == CaseStatuses.SentToBranchManager);
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);
            }

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.BranchManager.Name))
            {
                var newQuery = query.Where(c => (c.Status == CaseStatuses.SentToBranchManager || c.Status == CaseStatuses.ReceivedByBranchManager || c.Status == CaseStatuses.ReceivedByResearcher)
                && c.BranchId == CurrentUser.BranchId);
                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);
            }

            // check if user is litigationmanager
            var isLitigationManager = await db.AppUserRoleDepartmets.Where(u => u.UserId == CurrentUser.UserId
            && u.RoleId == ApplicationRolesConstants.DepartmentManager.Code
            && u.DepartmentId == 1).AnyAsync();

            if (isLitigationManager)
            {

                IQueryable<Case> newQuery = null;

                if (queryObject.IsForChooseRelatedCase == true)
                    newQuery = query.Where(c => c.BranchId == CurrentUser.BranchId && (c.Status == CaseStatuses.DoneJudgment || c.Status == CaseStatuses.ClosedCase || c.Status == CaseStatuses.ObjectionRecorded));
                else
                    newQuery = query.Where(c => c.BranchId == CurrentUser.BranchId && c.Status != CaseStatuses.Draft);

                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);

            }

            if (CurrentUser.Roles.Contains(ApplicationRolesConstants.GeneralSupervisor.Name))
            {
                var ignoredStatuses = new CaseStatuses[] { CaseStatuses.Draft, CaseStatuses.Deleted };

                IQueryable<Case> newQuery = query.Where(m => !ignoredStatuses.Contains(m.Status));

                if (finalQuery == null) finalQuery = newQuery; else finalQuery = finalQuery.Union(newQuery);
            }
              
            // if final query equal null
            if (finalQuery == null)
                finalQuery = query.Where(x => false);

            finalQuery = FilterCases(finalQuery, queryObject);

            var columnsMap = new Dictionary<string, Expression<Func<Case, object>>>()
            {
                ["id"] = v => v.Id,
                ["caseSource"] = v => v.CaseSource,
                ["court"] = v => v.Court.Name,
                ["status"] = v => v.Status,
                ["createdOn"] = v => v.CreatedOn
            };

            finalQuery = finalQuery.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await finalQuery.CountAsync();

            finalQuery = finalQuery.ApplyPaging(queryObject);

            result.Items = await finalQuery.AsNoTracking().ToListAsync();

            return mapper.Map<QueryResult<Case>, QueryResultDto<CaseListItemDto>>(result);
        }

        public List<CaseDto> GetAllAsync()
        {
            var result = new List<CaseDto>();
            var isCurrentUserResearcher = db.Users.Include(u => u.UserRoles)
                .Where(u => u.Id == CurrentUser.UserId && u.UserRoles.Any(m => m.RoleId == ApplicationRolesConstants.LegalResearcher.Code)).Any();

            var query = db.Cases
                .Include(m => m.Hearings)
                .Include(m => m.Parties)
                    .ThenInclude(p => p.Party)
                .Include(c => c.Court)
                .Include(t => t.Researchers)
                .ThenInclude(r => r.Researcher)
                .AsQueryable();

            if (isCurrentUserResearcher)
            {
                query = query.Where(c => c.Researchers.Any() && c.Researchers.Where(r => r.ResearcherId == CurrentUser.UserId).Any());
            }

            foreach (var c in query)
            {
                var caseItem = mapper.Map<Case, CaseDto>(c);
                result.Add(caseItem);

            }
            return result;
        }

        public async Task<CaseDetailsDto> GetAsync(int id, bool includeAllData = true)
        {
            var query = db.Cases.IgnoreQueryFilters().AsQueryable();
            if (includeAllData)
            {
                query = query.Include(cm => cm.CaseMoamalat)
                   .ThenInclude(m => m.Moamala)
               .Include(m => m.Parties)
                   .ThenInclude(p => p.Party)
               .Include(m => m.Parties)
                   .ThenInclude(p => p.Party)
                       .ThenInclude(p => p.IdentityType)
               .Include(c => c.Court)
               .Include(g => g.CaseGrounds)
               .Include(t => t.CaseTransactions)
                   .ThenInclude(u => u.CreatedByUser)
                       .ThenInclude(ur => ur.UserRoles)
                           .ThenInclude(r => r.Role)
               .Include(t => t.Researchers)
                   .ThenInclude(r => r.Researcher)
               .Include(h => h.Hearings)
                   .ThenInclude(a => a.Attachments)
               .Include(h => h.Hearings)
                   .ThenInclude(a => a.Court)
               .Include(r => r.CaseRule)
                   .ThenInclude(cr => cr.CaseRuleMinistryDepartments)
                       .ThenInclude(cr => cr.MinistryDepartment)
               .Include(r => r.CaseRule)
                   .ThenInclude(cr => cr.Attachments)
                       .ThenInclude(a => a.Attachment)
               .Include(r => r.CaseRule)
                   .ThenInclude(cr => cr.Attachments)
                       .ThenInclude(cr => cr.Attachment)
                           .ThenInclude(a => a.AttachmentType)
               .Include(r => r.CaseRule)
                   .ThenInclude(cr => cr.MinistrySector)
               .Include(c => c.RelatedCase)
                   .ThenInclude(c => c.Court)
               .Include(c => c.RelatedCase)
                    .ThenInclude(c => c.Branch)
                .Include(c => c.RelatedCase)
                   .ThenInclude(m => m.Parties)
                     .ThenInclude(m => m.Party)
                .Include(c => c.RelatedCase)
                   .ThenInclude(g => g.CaseGrounds)
                .Include(c => c.RelatedCase)
                   .ThenInclude(r => r.CaseRule)
               .Include(m => m.Attachments)
                   .ThenInclude(cr => cr.Attachment)
                      .ThenInclude(m => m.AttachmentType)
               .Include(m => m.Attachments)
                  .ThenInclude(m => m.Attachment)
               .Include(c => c.SecondSubCategory)
                   .ThenInclude(cc => cc.FirstSubCategory)
                       .ThenInclude(cc => cc.MainCategory)
               .AsNoTracking();
            }
            var _case = await query.FirstOrDefaultAsync(s => s.Id == id);

            return mapper.Map<Case, CaseDetailsDto>(_case);
        }

        public async Task AddAsync(BasicCaseDto caseDto)
        {
            var entityToAdd = mapper.Map<Case>(caseDto);
            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.BranchId = CurrentUser.BranchId;

            await db.Cases.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // get case id to use it in attachment
            caseDto.Id = entityToAdd.Id;
        }

        public async Task AddNextAsync(NextCaseDto caseDTO)
        {

            // update related case status  
            var relatedCase = await db.Cases.Include(c => c.Parties).Include(c => c.CaseGrounds).Include(c => c.Researchers).FirstOrDefaultAsync(c => c.Id == caseDTO.RelatedCaseId);
            relatedCase.Status = CaseStatuses.ClosedCase;
            var entityToAdd = mapper.Map<Case>(relatedCase);

            entityToAdd.CaseGrounds = entityToAdd.CaseGrounds.Select(i =>
            {
                i.CreatedBy = CurrentUser.UserId;
                return i;
            }).ToList();
            entityToAdd.Parties = entityToAdd.Parties.Select(i =>
            {
                i.CreatedBy = CurrentUser.UserId;
                return i;
            }).ToList();
            entityToAdd.Researchers = entityToAdd.Researchers.Select(i =>
            {
                i.CreatedBy = CurrentUser.UserId;
                return i;
            }).ToList();

            // add next case data 
            entityToAdd.RelatedCaseId = caseDTO.RelatedCaseId;
            entityToAdd.CaseNumberInSource = caseDTO.CaseNumberInSource;
            entityToAdd.CaseYearInSource = caseDTO.StartDate.Year;
            entityToAdd.CircleNumber = caseDTO.CircleNumber;
            entityToAdd.CourtId = caseDTO.CourtId;
            entityToAdd.LitigationType = relatedCase.LitigationType == LitigationTypes.FirstInstance ? LitigationTypes.Appeal : LitigationTypes.Supreme;
            entityToAdd.Status = CaseStatuses.ReceivedByResearcher;
            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.StartDate = caseDTO.StartDate;
            entityToAdd.ReceivingJudgmentDate = null;
            entityToAdd.PronouncingJudgmentDate = null;

            await db.Cases.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map(entityToAdd, caseDTO);
        }

        public async Task AddIntegratedCaseAsync(CaseDto caseDto)
        {
            var entityToAdd = mapper.Map<Case>(caseDto);
            entityToAdd.CreatedBy = ApplicationConstants.SystemAdministratorId;
            entityToAdd.CreatedOn = DateTime.Now;

            foreach (var party in entityToAdd.Parties)
            {
                party.CreatedBy = ApplicationConstants.SystemAdministratorId;
                party.CreatedOn = DateTime.Now;
            }

            foreach (var hearing in entityToAdd.Hearings)
            {
                hearing.Status = HearingStatuses.Scheduled;
                hearing.CreatedBy = ApplicationConstants.SystemAdministratorId;
                hearing.CreatedOn = DateTime.Now;
            }

            await db.Cases.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map(entityToAdd, caseDto);
        }

        public async Task EditAsync(BasicCaseDto caseDto)
        {
            var entityToUpdate = await db.Cases
                .FirstOrDefaultAsync(s => s.Id == caseDto.Id);

            await AddCaseHistoryAsync(entityToUpdate);

            mapper.Map(caseDto, entityToUpdate);

            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var entityToDelete = await db.Cases.FindAsync(id);

            await AddCaseHistoryAsync(entityToDelete);

            entityToDelete.UpdatedBy = CurrentUser.UserId;
            entityToDelete.UpdatedOn = DateTime.Now;
            entityToDelete.IsDeleted = true;
            entityToDelete.Status = CaseStatuses.Deleted;

            var hearings = await db.Hearings.Where(x => x.CaseId == id).ToListAsync();
            foreach (var hearing in hearings)
            {
                hearing.UpdatedBy = CurrentUser.UserId;
                hearing.UpdatedOn = DateTime.Now;
                hearing.IsDeleted = true;
            }

            var grounds = await db.CaseGrounds.Where(x => x.CaseId == id).ToListAsync();
            foreach (var ground in grounds)
            {
                ground.UpdatedBy = CurrentUser.UserId;
                ground.UpdatedOn = DateTime.Now;
                ground.IsDeleted = true;
            }

            var parties = await db.CaseParties.Where(x => x.CaseId == id).ToListAsync();
            foreach (var party in parties)
            {
                party.UpdatedBy = CurrentUser.UserId;
                party.UpdatedOn = DateTime.Now;
                party.IsDeleted = true;
            }

            var caseAttachments = await db.CaseAttachments.Include(x => x.Attachment).Where(x => x.CaseId == id).ToListAsync();
            foreach (var attachment in caseAttachments)
            {
                attachment.Attachment.IsDeleted = true;
            }

            await db.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(CaseChangeStatusDto caseChangeStatusDto)
        {
            var entityToUpdate = await db.Cases.FindAsync(caseChangeStatusDto.Id);

            await AddCaseHistoryAsync(entityToUpdate);

            entityToUpdate.Status = caseChangeStatusDto.Status;
            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();
        }

        #region Case Party
        public async Task<List<CasePartyDto>> GetCasePartiesAsync(int caseId)
        {
            var parties = await db.CaseParties.Where(c => c.CaseId == caseId)
                .Include(x => x.Party)
                .ToListAsync();

            return mapper.Map<List<CaseParty>, List<CasePartyDto>>(parties);
        }

        public async Task AddCasePartyAsync(CasePartyDto partyDto)
        {
            partyDto.Party = null;
            var entityToAdd = mapper.Map<CaseParty>(partyDto);

            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;

            await db.CaseParties.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            partyDto.Id = entityToAdd.Id;
        }

        public async Task UpdateCasePartyAsync(CasePartyDto partyDto)
        {
            var entityToUpdate = await db.CaseParties
                .FirstOrDefaultAsync(s => s.Id == partyDto.Id);

            partyDto.Party = null;
            mapper.Map(partyDto, entityToUpdate);

            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();
        }

        public async Task DeleteCasePartyAsync(int id)
        {
            var entityToDelete = await db.CaseParties.FindAsync(id);

            entityToDelete.UpdatedBy = CurrentUser.UserId;
            entityToDelete.UpdatedOn = DateTime.Now;
            entityToDelete.IsDeleted = true;

            await db.SaveChangesAsync();
        }

        #endregion

        #region Case Ground
        public async Task<ICollection<CaseGroundsDto>> GetAllGroundsAsync(int caseId)
        {
            var grounds = await db.CaseGrounds.Where(c => c.CaseId == caseId).ToListAsync();
            return mapper.Map<List<CaseGrounds>, List<CaseGroundsDto>>(grounds);
        }

        public async Task AddGroundAsync(CaseGroundsDto caseGroundsDto)
        {
            var entityToAdd = mapper.Map<CaseGrounds>(caseGroundsDto);

            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;

            await db.CaseGrounds.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            caseGroundsDto.Id = entityToAdd.Id;
        }

        public async Task EditGroundAsync(CaseGroundsDto caseGroundsDto)
        {
            var entityToUpdate = await db.CaseGrounds
                .FirstOrDefaultAsync(s => s.Id == caseGroundsDto.Id);

            mapper.Map(caseGroundsDto, entityToUpdate);

            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();
        }

        public async Task RemoveGroundAsync(int id)
        {
            var entityToDelete = await db.CaseGrounds.FindAsync(id);

            entityToDelete.UpdatedOn = DateTime.Now;
            entityToDelete.IsDeleted = true;

            await db.SaveChangesAsync();
        }

        public async Task UpdateAllGroundsAsync(CaseGroundsListDto caseGrounds)
        {
            var oldGrounds = await db.CaseGrounds.Where(c => c.CaseId == caseGrounds.CaseId).ToListAsync();
            db.CaseGrounds.RemoveRange(oldGrounds);

            var caseGroundsToAdd = new List<CaseGrounds>();

            foreach (var ground in caseGrounds.CaseGrounds)
                caseGroundsToAdd.Add(new CaseGrounds() { CaseId = caseGrounds.CaseId, Text = ground.Text });

            await db.CaseGrounds.AddRangeAsync(caseGroundsToAdd);

            await db.SaveChangesAsync();
        }

        #endregion

        #region Case Moamalat

        public async Task<List<CaseMoamalatDto>> GetCaseMoamalatAsync(int caseId)
        {
            var moamalat = await db.CaseMoamalat.Where(c => c.CaseId == caseId)
                .Include(x => x.Moamala)
                .ToListAsync();

            return mapper.Map<List<CaseMoamala>, List<CaseMoamalatDto>>(moamalat);
        }

        public async Task AddCaseMoamalatAsync(CaseMoamalatDto caseMoamalatDto)
        {
            caseMoamalatDto.Moamala = null;
            var entityToAdd = mapper.Map<CaseMoamala>(caseMoamalatDto);

            entityToAdd.CreatedOn = DateTime.Now;

            await db.CaseMoamalat.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
        }

        public async Task RemoveCaseMoamalatAsync(int caseId, int moamalaId)
        {
            var entityToDelete = await db.CaseMoamalat
                .Where(c => c.CaseId == caseId && c.MoamalaId == moamalaId).ToListAsync();

            db.CaseMoamalat.RemoveRange(entityToDelete);

            await db.SaveChangesAsync();
        }

        #endregion

        public async Task AddCaseHistoryAsync(Case caseEntity)
        {
            var entityToAdd = mapper.Map<CaseHistory>(caseEntity);

            await db.CaseHistory.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
        }

        public async Task EditAttachmentsAsync(CaseAttachmentsListDto caseDto)
        {
            var entityToUpdate = await db.Cases.Include(c => c.Attachments)
                .FirstOrDefaultAsync(s => s.Id == caseDto.CaseId);

            //
            var attachmentsToAdd = caseDto.Attachments.Where(a => !a.IsDeleted).ToList();

            var removedAttachments = entityToUpdate.Attachments.Where(a => !attachmentsToAdd.Select(d => d.Id).Contains(a.Id)).ToList();

            db.CaseAttachments.RemoveRange(removedAttachments);

            foreach (var attachment in attachmentsToAdd)
                if (!entityToUpdate.Attachments.Select(a => (Guid?)a.Id).Contains(attachment.Id))
                    entityToUpdate.Attachments.Add(new CaseAttachment
                    {
                        CaseId = entityToUpdate.Id,
                        Id = (Guid)attachment.Id
                    });

            await db.SaveChangesAsync();
        }

        public async Task<CaseDetailsDto> ChangeDepartment(CaseSendToBranchManagerDto caseSendToBranchManagerDto)
        {
            var entityToUpdate = await db.Cases.FindAsync(caseSendToBranchManagerDto.Id);

            await AddCaseHistoryAsync(entityToUpdate);

            entityToUpdate.Status = CaseStatuses.SentToBranchManager;
            entityToUpdate.BranchId = caseSendToBranchManagerDto.BranchId;
            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;

            await db.SaveChangesAsync();
            return mapper.Map<Case, CaseDetailsDto>(entityToUpdate);
        }

        public async Task<CaseDetailsDto> GetParentCase(int id)
        {
            var _case = await db.Cases
             .FirstOrDefaultAsync(a => a.RelatedCaseId == id);

            if (_case != null)
            {
                return mapper.Map<Case, CaseDetailsDto>(_case);
            }

            return null;
        }

        private IQueryable<Case> FilterCases(IQueryable<Case> query, CaseQueryObject queryObject)
        {
            if (queryObject.StartDateFrom.HasValue)
            {
                query = query.Where(p => p.StartDate.Date >= queryObject.StartDateFrom.Value.Date);
            }

            if (queryObject.StartDateTo.HasValue)
            {
                query = query.Where(p => p.StartDate.Date <= queryObject.StartDateTo.Value.Date);
            }

            if (queryObject.CaseSource.HasValue)
            {
                query = query.Where(c => (int)c.CaseSource == queryObject.CaseSource);
            }

            if (queryObject.LegalStatus.HasValue)
            {
                query = query.Where(c => (int)c.LegalStatus == queryObject.LegalStatus);
            }

            if (queryObject.LitigationType.HasValue)
            {
                query = query.Where(c => (int)c.LitigationType == queryObject.LitigationType);
            }

            if (queryObject.Status.HasValue)
            {
                query = query.Where(c => (int)c.Status == queryObject.Status);
            }

            if (!string.IsNullOrEmpty(queryObject.CourtId))
            {
                query = query.Where(c => queryObject.CourtId.Split(new char[] { ',' }).Contains(c.CourtId.ToString()));
            }

            if (!string.IsNullOrEmpty(queryObject.CircleNumber))
            {
                query = query.Where(c => c.CircleNumber == queryObject.CircleNumber);
            }

            if (queryObject.AddUserId != null)
            {
                query = query.Where(c => c.CreatedBy == queryObject.AddUserId);
            }

            if (queryObject.IsCaseDataCompleted.HasValue)
            {
                query = query.Where(c => c.Subject != null && c.CourtId != null);
            }

            if (queryObject.IsForHearingAddition == true)
            {
                query = query.Where(c => c.ReceivingJudgmentDate == null && c.Status != CaseStatuses.DoneJudgment && c.Status != CaseStatuses.ClosedCase && c.Status != CaseStatuses.ObjectionRecorded
                && c.Researchers.Any(r => r.ResearcherId == CurrentUser.UserId));
            }

            if (queryObject.Period.HasValue)
            {
                query = query.Where(p => p.CreatedOn >= DateTime.Now.AddDays(-1 * queryObject.Period.Value));
            }

            if (!string.IsNullOrEmpty(queryObject.ReferenceCaseNo))
            {
                query = query.Where(c => c.ReferenceCaseNo == queryObject.ReferenceCaseNo);
            }

            if (!string.IsNullOrEmpty(queryObject.Subject))
            {
                query = query.Where(c => c.Subject.Contains(queryObject.Subject));
            }

            if (queryObject.IsManual.HasValue)
            {
                query = query.Where(c => c.IsManual == queryObject.IsManual);
            }

            if (!string.IsNullOrEmpty(queryObject.SearchText))
            {
                query = query.Where(c => c.Id.ToString().Contains(queryObject.SearchText)
                                      || c.CreatedOn.Date.ToString().Contains(queryObject.SearchText)
                                      || c.Court.Name.ToLower().Contains(queryObject.SearchText)
                                      || c.CaseNumberInSource.Contains(queryObject.SearchText)
                                      || c.CircleNumber.Contains(queryObject.SearchText)
                                      || c.Subject.Contains(queryObject.SearchText)
                                      || c.CaseDescription.Contains(queryObject.SearchText));
            }

            if (!string.IsNullOrEmpty(queryObject.PartyName))
            {
                query = query.Where(c => c.Parties.Any(p => p.Party.Name.Contains(queryObject.PartyName)));
            }

            if (queryObject.IsFinalJudgment.HasValue)
            {
                query = query.Where(c => c.CaseRule.IsFinalJudgment == queryObject.IsFinalJudgment.Value);
            }

            if (queryObject.IsClosedCase.HasValue)
            {
                if (queryObject.IsClosedCase.Value)
                    query = query.Where(c => c.Status == CaseStatuses.ClosedCase);
                else
                    query = query.Where(c => c.Status != CaseStatuses.ClosedCase);
            }

            if (queryObject.LegalMemoType.HasValue)
            {
                if (queryObject.LegalMemoType == LegalMemoTypes.Pleading)
                { query = query.Where(c => c.Status == CaseStatuses.ReceivedByResearcher); }
                if (queryObject.LegalMemoType == LegalMemoTypes.Objection)
                { query = query.Where(c => c.Status == CaseStatuses.DoneJudgment); }
            }

            return query;
        }

        public async Task AddRuleAsync(CaseRuleDto caseRuleDto)
        {
            var entityToAdd = mapper.Map<CaseRule>(caseRuleDto);
            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;

            await db.CaseRules.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map(entityToAdd, caseRuleDto);
        }

        public async Task AddCategory(int caseId, int categoryId)
        {
            var entityToAdd = db.Cases.Where(c => c.Id == caseId).FirstOrDefault();
            entityToAdd.SecondSubCategoryId = categoryId;
            await db.Cases.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
        }

        public async Task ReceiveJudgmentInstrumentAsync(ReceiveJdmentInstrumentDto receiveJdmentInstrumentDto)
        {
            var caseToUpdate = await db.Cases.FindAsync(receiveJdmentInstrumentDto.Id);
            caseToUpdate.Status = CaseStatuses.DoneJudgment;

            var entityToAdd = mapper.Map<CaseRule>(receiveJdmentInstrumentDto);
            entityToAdd.IsFinalJudgment = false;
            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;
            await db.CaseRules.AddAsync(entityToAdd);
            await db.SaveChangesAsync();
        }

        public async Task RemoveJudgmentInstrumentAsync(int caseId)
        {
            var caseToUpdate = await db.Cases
                                    .Include(c => c.CaseRule)
                                    .ThenInclude(a => a.Attachments)
                                    .Where(m => m.Id == caseId).FirstOrDefaultAsync();
            caseToUpdate.Status = CaseStatuses.ReceivedByResearcher;
            if (caseToUpdate.Attachments.Count > 0)
            {
                db.CaseRuleAttachments.RemoveRange(caseToUpdate.CaseRule.Attachments);
            }
            if (caseToUpdate.CaseRule != null)
            {
                db.CaseRules.Remove(caseToUpdate.CaseRule);
            }
            await db.SaveChangesAsync();
        }

        public async Task<ReceiveJdmentInstrumentDetailsDto> GetReceiveJudgmentInstrumentAsync(int Id)
        {
            var _caseRule = await db.CaseRules
                .Include(c => c.Attachments)
                  .ThenInclude(a => a.Attachment)
                .Include(c => c.Attachments)
                  .ThenInclude(a => a.Attachment)
                  .ThenInclude(a => a.AttachmentType)
                .Include(c => c.CaseRuleMinistryDepartments)
                .ThenInclude(d => d.MinistryDepartment)
            .FirstOrDefaultAsync(a => a.Id == Id);

            return mapper.Map<ReceiveJdmentInstrumentDetailsDto>(_caseRule);
        }

        public async Task EditReceiveJudgmentInstrumentAsync(ReceiveJdmentInstrumentDto receiveJdmentInstrumentDto)
        {
            var entityToUpdate = await db.CaseRules
                .Include(c => c.Attachments)
                .ThenInclude(a => a.Attachment)
                .Include(c => c.CaseRuleMinistryDepartments)
                .ThenInclude(c => c.MinistryDepartment)
            .FirstOrDefaultAsync(a => a.Id == receiveJdmentInstrumentDto.Id);
            mapper.Map(receiveJdmentInstrumentDto, entityToUpdate);
            entityToUpdate.UpdatedBy = CurrentUser.UserId;
            entityToUpdate.UpdatedOn = DateTime.Now;
            await db.SaveChangesAsync();
        }

        public async Task<List<Case>> DetermineJudgment()
        {
            var ignoredCaseStatuses = new[] { CaseStatuses.ClosedCase, CaseStatuses.ObjectionRecorded };

            var _cases = await db.Cases
           .Include(c => c.CaseRule)
           .Where(c => c.CaseRule != null && !c.CaseRule.IsFinalJudgment.Value
               && !c.IsArchived
               && !ignoredCaseStatuses.Contains(c.Status)
               && (c.LitigationType == LitigationTypes.Supreme || c.ReceivingJudgmentDate.Value.AddDays(30) < DateTime.Now))
           .ToListAsync();

            return _cases;
        }

        public async Task UpdateDetermineJudment(Case _case)
        {
            _case.CaseRule.IsFinalJudgment = true;
            _case.Status = CaseStatuses.ClosedCase;
            _case.CaseClosingReason = CaseClosinReasons.EndOfObjectionPeriod;
            _case.UpdatedBy = CurrentUser.UserId;
            _case.UpdatedOn = DateTime.Now;
            await db.SaveChangesAsync();
        }

        public async Task<JudgementResults> GetJudgmentResult(int caseId)
        {
            var judgmentResult = await db.CaseRules.Where(c => c.Id == caseId).Select(c => c.JudgementResult).FirstOrDefaultAsync();
            return (JudgementResults)judgmentResult;
        }

        public async Task<bool> IsCategoryBelongsToAnyCaseAsync(int secondSubCategoryId)
        {
            return await db.Cases.AnyAsync(x => x.SecondSubCategoryId == secondSubCategoryId) ? true : false;
        }

        public async Task CreateAsync(InitialCaseDto caseDto)
        {
            var entityToAdd = mapper.Map<Case>(caseDto);
            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.Status = CaseStatuses.NewCase;
            entityToAdd.IsManual = true;
            entityToAdd.BranchId = CurrentUser.BranchId;

            await db.Cases.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // get case id to use it in attachment
            caseDto.Id = entityToAdd.Id;
        }

        public async Task<bool> IsCaseSourceNumberExistsAsync(CaseSources caseSource, string caseSourceNumber, DateTime startDate)
        {
            return await db.Cases
                .AnyAsync(c => !c.IsDeleted
                && c.CaseSource == caseSource
                && c.CaseNumberInSource == caseSourceNumber
                && c.StartDate.Year == startDate.Year);
        }
        public async Task<bool> IsCaseStartDateValidAsync(int relatedCaseId, DateTime startDate)
        {
            return await db.Cases
                .AnyAsync(c => !c.IsDeleted
                && c.Id == relatedCaseId
                && c.PronouncingJudgmentDate < startDate);
        }

        public async Task<ObjectionCaseDto> CreateObjectionCase(ObjectionCaseDto objectionCaseDto)
        {
            var oldCase = await db.Cases
                .Include(c => c.SecondSubCategory)
                    .ThenInclude(c => c.FirstSubCategory)
                        .ThenInclude(c => c.MainCategory)
                .Include(c => c.Parties)
                    .ThenInclude(p => p.Party)
                .Where(c => c.Id == objectionCaseDto.RelatedCaseId)
                .FirstOrDefaultAsync();

            var entityToAdd = mapper.Map<Case>(objectionCaseDto);

            entityToAdd.CreatedBy = CurrentUser.UserId;
            entityToAdd.CreatedOn = DateTime.Now;
            entityToAdd.Status = CaseStatuses.ReceivedByResearcher;
            entityToAdd.IsManual = true;
            entityToAdd.BranchId = CurrentUser.BranchId;
            entityToAdd.LegalStatus = oldCase.LegalStatus;
            entityToAdd.CaseSource = oldCase.CaseSource;
            entityToAdd.SecondSubCategoryId = oldCase.SecondSubCategory.Id;

            await db.Cases.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            // get case id to use it in attachment
            objectionCaseDto.Id = entityToAdd.Id;

            foreach (var item in oldCase.Parties)
            {
                item.Id = 0;
                item.CaseId = entityToAdd.Id;
                await db.CaseParties.AddAsync(item);
            }
            await db.SaveChangesAsync();
            objectionCaseDto.ResearcherId = CurrentUser.UserId;
            objectionCaseDto.GeneralManagmentId = entityToAdd.BranchId;
            return objectionCaseDto;
        }

        public async Task<List<Case>> GetNotFinalJudjmentCases(int days)
        {
            var ignoredCaseStatuses = new[] { CaseStatuses.ClosedCase, CaseStatuses.ObjectionRecorded };

            var _cases = await db.Cases
               .Include(c => c.CaseRule)
               .Include(c => c.Court)
               .Where(c => c.CaseRule != null
                   && !c.IsArchived
                   && !ignoredCaseStatuses.Contains(c.Status)
                   && c.ReceivingJudgmentDate.Value.AddDays(days).Date == DateTime.Now.Date)
               .ToListAsync();

            return _cases;
        }

        public async Task<int> GetCaseByRelatedId(int relatedCaseId)
        {
            var _caseId = await db.Cases
                .AsNoTracking()
                .Where(s => s.RelatedCaseId == relatedCaseId)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            return _caseId;
        }

        public async Task<LitigationTypes?> GetCaseLititgationType(int caseId)
        {
            var _caseLitigationType = await db.Cases
                .AsNoTracking()
                .Where(c => c.Id == caseId)
                .Select(c => c.LitigationType)
                .FirstOrDefaultAsync();

            return _caseLitigationType;
        }
    }
}
