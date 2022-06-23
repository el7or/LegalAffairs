using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IInvestigationRecordRepository
    {
        Task<QueryResultDto<InvestigationRecordListItemDto>> GetAllAsync(InvestiationRecordQueryObject queryObject);

        Task<InvestigationRecordDetailsDto> GetAsync(int id);

        Task AddAsync(InvestigationRecordDto InvestigationRecord);

        Task EditAsync(InvestigationRecordDto InvestigationRecord);

        Task RemoveAsync(int id);

        Task<bool> checkPartyExist(int investigationRecordPartyId, int investigationRecordId);
    }

}
