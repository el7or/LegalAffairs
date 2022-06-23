using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IInvestigationRecordPartyTypeService
    {
        Task<ReturnResult<QueryResultDto<InvestigationRecordPartyTypeListItemDto>>> GetAllAsync(QueryObject queryObject);

        Task<ReturnResult<InvestigationRecordPartyTypeListItemDto>> GetAsync(int id);

        Task<ReturnResult<InvestigationRecordPartyTypeDto>> AddAsync(InvestigationRecordPartyTypeDto model);

        Task<ReturnResult<InvestigationRecordPartyTypeDto>> EditAsync(InvestigationRecordPartyTypeDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(string name);
    }
}
