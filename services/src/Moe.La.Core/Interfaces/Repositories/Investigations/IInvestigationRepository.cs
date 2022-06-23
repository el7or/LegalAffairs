using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IInvestigationRepository
    {
        Task<QueryResultDto<InvestigationListItemDto>> GetAllAsync(InvestigationQueryObject queryObject);

        Task<InvestigationDetailsDto> GetAsync(int id);

        Task AddAsync(InvestigationDto InvestigationRecord);

        Task EditAsync(InvestigationDto InvestigationRecord);

        Task RemoveAsync(int id);
    }

}
