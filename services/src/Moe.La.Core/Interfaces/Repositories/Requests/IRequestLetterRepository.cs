using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IRequestLetterRepository
    {
        Task<QueryResultDto<RequestLetterDto>> GetAllAsync(QueryObject queryObject);

        Task<RequestLetterDto> GetAsync(int id);

        Task<RequestLetterDto> GetByRequestIdAsync(int id);

        Task AddAsync(RequestLetterDto requestLetterDto);

        Task EditAsync(RequestLetterDto requestLetterDto);

        Task RemoveAsync(int id);

        Task<String> ReplaceDocumentRequestContent(int templateId, CaseDetailsDto _case, CaseSupportingDocumentRequestListItemDto request);

        Task<String> ReplaceCaseCloseRequestContent(int templateId, CaseDetailsDto _case);
        Task<String> ReplaceCaseContent(int templateId, CaseDetailsDto _case);
    }
}