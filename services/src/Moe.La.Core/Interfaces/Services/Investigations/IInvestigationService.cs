using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IInvestigationService
    {
        Task<ReturnResult<QueryResultDto<InvestigationListItemDto>>> GetAllAsync(InvestigationQueryObject queryObject);

        Task<ReturnResult<InvestigationDetailsDto>> GetAsync(int id);

        Task<ReturnResult<InvestigationDto>> AddAsync(InvestigationDto model);

        Task<ReturnResult<InvestigationDto>> EditAsync(InvestigationDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);
    }
}
