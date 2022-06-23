using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IMinistrySectorRepository
    {
        Task<QueryResultDto<MinistrySectorListItemDto>> GetAllAsync(QueryObject queryObject);

        Task<MinistrySectorListItemDto> GetAsync(int id);

        Task EditAsync(MinistrySectorDto ministrySectorDto);

        Task RemoveAsync(int id, Guid userId);

        Task AddAsync(MinistrySectorDto ministrySectorDto);

        Task<int> GetSectorIdAsync(string Name);

        Task<bool> IsNameExistsAsync(string name);
    }
}
