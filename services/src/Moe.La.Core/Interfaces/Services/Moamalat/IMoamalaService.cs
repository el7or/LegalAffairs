using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IMoamalaService
    {
        Task<ReturnResult<QueryResultDto<MoamalaListItemDto>>> GetAllAsync(MoamalatQueryObject queryObject);

        Task<ReturnResult<MoamalaDetailsDto>> GetAsync(int id);

        Task<ReturnResult<MoamalaDto>> AddAsync(MoamalaDto model);

        Task<ReturnResult<MoamalaDto>> EditAsync(MoamalaDto model);

        Task<ReturnResult<bool>> RemoveAsync(int id);

        Task<ReturnResult<bool>> ChangeStatusAsync(MoamalaChangeStatusDto changeStatusDto);

        Task<ReturnResult<bool>> ReturnAsync(int id, string note);

        Task<ReturnResult<MoamalaDetailsDto>> UpdateWorkItemTypeAsync(MoamalaUpdateWorkItemType model);

        Task<ReturnResult<MoamalaDetailsDto>> UpdateRelatedIdAsync(MoamalaUpdateRelatedId model);

        Task<ReturnResult<MoamalaTransactionDto>> AddTransactionAsync(MoamalaTransactionDto model);

    }
}
