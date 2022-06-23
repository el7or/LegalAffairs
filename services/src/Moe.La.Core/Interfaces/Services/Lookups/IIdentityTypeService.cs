using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IIdentityTypeService
    {
        Task<ReturnResult<QueryResultDto<IdentityTypeListItemDto>>> GetAllAsync(QueryObject queryObject);

        Task<ReturnResult<IdentityTypeListItemDto>> GetAsync(int id);

        Task<ReturnResult<IdentityTypeDto>> AddAsync(IdentityTypeDto model);

        Task<ReturnResult<IdentityTypeDto>> EditAsync(IdentityTypeDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> IsNameExistsAsync(string name);
    }
}
