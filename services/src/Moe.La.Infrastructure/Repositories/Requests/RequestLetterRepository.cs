using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moe.La.Common;
using Moe.La.Core.Common;
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
    public class RequestLetterRepository : RepositoryBase, IRequestLetterRepository
    {
        public RequestLetterRepository(LaDbContext commandDb, IMapper mapperConfig, IUserProvider userProvider)
            : base(commandDb, mapperConfig, userProvider)
        {
        }

        public async Task<QueryResultDto<RequestLetterDto>> GetAllAsync(QueryObject queryObject)
        {
            var result = new QueryResult<RequestLetter>();

            var query = db.RequestLetters
                 .AsQueryable();


            var columnsMap = new Dictionary<string, Expression<Func<RequestLetter, object>>>()
            {
                ["id"] = v => v.RequestId
            };

            query = query.ApplySorting(queryObject, columnsMap);

            result.TotalItems = await query.CountAsync();

            query = query.ApplyPaging(queryObject);

            result.Items = await query.AsNoTracking().ToListAsync();

            return mapper.Map<QueryResult<RequestLetter>, QueryResultDto<RequestLetterDto>>(result);
        }

        public async Task<RequestLetterDto> GetAsync(int id)
        {
            var entity = await db.RequestLetters
                .Where(x => x.RequestId == id).FirstOrDefaultAsync();

            return mapper.Map<RequestLetterDto>(entity);
        }

        public async Task AddAsync(RequestLetterDto requestLetterDto)
        {
            var entityToAdd = mapper.Map<RequestLetter>(requestLetterDto);
            await db.RequestLetters.AddAsync(entityToAdd);
            await db.SaveChangesAsync();

            mapper.Map(entityToAdd, requestLetterDto);
        }

        public async Task EditAsync(RequestLetterDto requestLetterDto)
        {
            var entityToUpdate = await db.RequestLetters.FindAsync(requestLetterDto.RequestId);
            mapper.Map(requestLetterDto, entityToUpdate);
            await db.SaveChangesAsync();

            mapper.Map(entityToUpdate, requestLetterDto);
        }

        public async Task RemoveAsync(int id)
        {
            var entityToDelete = await db.RequestLetters.FindAsync(id);
            db.RequestLetters.Remove(entityToDelete);
            await db.SaveChangesAsync();
        }

        public async Task<RequestLetterDto> GetByRequestIdAsync(int id)
        {
            var entity = await db.RequestLetters
                           .Where(x => x.RequestId == id).FirstOrDefaultAsync();

            return mapper.Map<RequestLetterDto>(entity);
        }

        public async Task<string> ReplaceDocumentRequestContent(int templateId, CaseDetailsDto _case, CaseSupportingDocumentRequestListItemDto request)
        {
            var content = await ReplaceCaseContent(templateId, _case);

            // for attached letter request
            content = content.Replace(TemplateLetterConstants.RequestNumber, request.Id.ToString());
            content = content.Replace(TemplateLetterConstants.RequestDate, DateTimeHelper.GetHigriDateForPrint(request.Request.CreatedOn).ToString());
            /// 

            content = content.Replace(TemplateLetterConstants.DepartmentName, "<span id='DepartmentName'>" + request.ConsigneeDepartment.Name + "</span>");

            return content;
        }
        public async Task<string> ReplaceCaseCloseRequestContent(int templateId, CaseDetailsDto _case)
        {
            var content = await ReplaceCaseContent(templateId, _case);

            content = content.Replace(TemplateLetterConstants.RequestNumber, "<span style='display:inline-block; width:100px;height:100px;background-color:'#f0f0f0'>##<span>");
            content = content.Replace(TemplateLetterConstants.RequestDate, "<span style='display:inline-block; width:100px;height:100px;background-color:'#f0f0f0'>##<span>");
            content = content.Replace(TemplateLetterConstants.DepartmentName, string.Join(" ، ", _case.CaseRule.CaseRuleMinistryDepartments.Select(m => m.Name)));

            return content;
        }


        public async Task<string> ReplaceCaseContent(int templateId, CaseDetailsDto _case)
        {
            var content = await db.LetterTemplates.Where(t => t.Id == templateId).Select(t => t.Text).FirstOrDefaultAsync();

            content = content.Replace(TemplateLetterConstants.CaseNumber, _case.CaseNumberInSource);
            content = content.Replace(TemplateLetterConstants.CaseSubject, _case.Subject);
            content = content.Replace(TemplateLetterConstants.CaseCourtName, _case.Court.Name);
            content = content.Replace(TemplateLetterConstants.CaseDate, DateTimeHelper.GetHigriYearInt(_case.StartDate).ToString());
            content = content.Replace(TemplateLetterConstants.HearingDate, DateTimeHelper.GetArDay(_case.Hearings.LastOrDefault().HearingDate) + " بتاريخ " + DateTimeHelper.GetHigriDateForPrint(_case.Hearings.LastOrDefault().HearingDate).ToString());
            content = content.Replace(TemplateLetterConstants.CaseJudgmentResult, _case.CaseRule?.JudgementResult?.Name);
            content = content.Replace(TemplateLetterConstants.CaseJudgmentText, _case.CaseRule?.JudgementText);
            content = content.Replace(TemplateLetterConstants.CircleNumber, _case.CircleNumber);
            content = content.Replace(TemplateLetterConstants.RuleNumber, _case.CaseRule?.RuleNumber == null ? "--" : _case.CircleNumber);
            content = content.Replace(TemplateLetterConstants.JudgeName, _case.JudgeName);
            content = content.Replace(TemplateLetterConstants.RuleDate, DateTimeHelper.GetHigriYearInt(_case.ReceivingJudgmentDate).ToString());

            if (_case.LegalStatus.Id == (int)MinistryLegalStatuses.Plaintiff)
                content = content.Replace(TemplateLetterConstants.Plaintiff, "الوزارة");
            else
            {
                content = content.Replace(TemplateLetterConstants.Plaintiff, PrintHelper.TransformParties(_case.Parties));
            }

            return content;
        }

    }
}
