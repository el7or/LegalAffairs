using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IInvestigationRecordPartyTypeRepository
    {
        Task<QueryResultDto<InvestigationRecordPartyTypeListItemDto>> GetAllAsync(QueryObject queryObject);

        Task<InvestigationRecordPartyTypeListItemDto> GetAsync(int id);

        Task AddAsync(InvestigationRecordPartyTypeDto investigationRecordPartyTypeDto);

        Task EditAsync(InvestigationRecordPartyTypeDto investigationRecordPartyTypeDto);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(string name);
    }
}
