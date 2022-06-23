using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IRequestLetterService
    {
        Task<ReturnResult<QueryResultDto<RequestLetterDto>>> GetAllAsync(QueryObject queryObject);

        Task<ReturnResult<RequestLetterDto>> GetAsync(int id);

        Task<ReturnResult<RequestLetterDto>> GetByRequestIdAsync(int id);

        Task<ReturnResult<RequestLetterDto>> AddAsync(RequestLetterDto requestLetterDto);

        Task<ReturnResult<RequestLetterDto>> EditAsync(RequestLetterDto requestLetterDto);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<string>> ReplaceDocumentRequestContent(int requestID, int templateID);

        Task<ReturnResult<string>> ReplaceCaseCloseRequestContent(int caseID, int templateID);

        ReturnResult<string> ReplaceDeplartment(string contnet, string departmentName);
    }
}
