using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IIdentityTypeRepository
    {
        Task<QueryResultDto<IdentityTypeListItemDto>> GetAllAsync(QueryObject queryObject);

        Task<IdentityTypeListItemDto> GetAsync(int id);

        Task AddAsync(IdentityTypeDto identityTypeDto);

        Task EditAsync(IdentityTypeDto saveIdentityTypeDto);

        Task RemoveAsync(int id);

        Task<bool> IsNameExistsAsync(string name);
    }
}