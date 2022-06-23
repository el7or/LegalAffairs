using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IInvestigationQuestionService
    {
        Task<ReturnResult<QueryResultDto<InvestigationQuestionListItemDto>>> GetAllAsync(InvestigationQuestionQueryObject queryObject);

        Task<ReturnResult<InvestigationQuestionListItemDto>> GetAsync(int id);

        Task<ReturnResult<InvestigationQuestionDto>> AddAsync(InvestigationQuestionDto model);

        Task<ReturnResult<InvestigationQuestionDto>> EditAsync(InvestigationQuestionDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<InvestigationQuestionDto>> ChangeQuestionStatusAsync(InvestigationQuestionChangeStatusDto model);

        Task<ReturnResult<bool>> IsNameExistAsync(string Question);

    }
}
