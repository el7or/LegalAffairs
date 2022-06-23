using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IRequestService
    {
        Task<ReturnResult<QueryResultDto<RequestListItemDto>>> GetAllAsync(RequestQueryObject queryObject);

        Task<ReturnResult<RequestListItemDto>> GetAsync(int id);

        Task<RequestForPrintDto> GetForPrintAsync(int id);

        Task<bool> UpdateStatusAsync(int id, RequestStatuses status);

        Task<ReturnResult<RequestTransactionDto>> AddTransactionAsync(RequestTransactionDto requestTransactionDto);

        Task<List<UserDetailsDto>> GetRequestApproveUsers(int requestId);
    }
}
