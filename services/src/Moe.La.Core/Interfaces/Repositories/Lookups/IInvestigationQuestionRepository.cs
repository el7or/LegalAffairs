using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IInvestigationQuestionRepository
    {
        Task<QueryResultDto<InvestigationQuestionListItemDto>> GetAllAsync(InvestigationQuestionQueryObject queryObject);

        Task<InvestigationQuestionListItemDto> GetAsync(int id);

        Task<InvestigationQuestionDto> AddAsync(InvestigationQuestionDto investigationQuestion);

        Task EditAsync(InvestigationQuestionDto investigationQuestion);

        Task<InvestigationQuestionDto> ChangeQuestionStatusAsync(InvestigationQuestionChangeStatusDto questionStatusDto);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(string question);
    }
}
