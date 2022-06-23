using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IInvestigationRecordService
    {
        Task<ReturnResult<QueryResultDto<InvestigationRecordListItemDto>>> GetAllAsync(InvestiationRecordQueryObject queryObject);

        Task<ReturnResult<InvestigationRecordDetailsDto>> GetAsync(int id);

        Task<ReturnResult<InvestigationRecordDto>> AddAsync(InvestigationRecordDto model);

        Task<ReturnResult<InvestigationRecordDto>> EditAsync(InvestigationRecordDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);
    }
}
