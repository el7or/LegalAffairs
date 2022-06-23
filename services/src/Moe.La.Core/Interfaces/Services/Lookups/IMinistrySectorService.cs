using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IMinistrySectorService
    {
        Task<ReturnResult<QueryResultDto<MinistrySectorListItemDto>>> GetAllAsync(QueryObject queryObject);

        Task<ReturnResult<MinistrySectorListItemDto>> GetAsync(int id);

        Task<ReturnResult<MinistrySectorDto>> AddAsync(MinistrySectorDto model);

        Task<ReturnResult<MinistrySectorDto>> EditAsync(MinistrySectorDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id, Guid userId);

        Task<ReturnResult<bool>> IsNameExistsAsync(string name);
    }
}
