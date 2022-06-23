using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IObjectionPermitRequestService
    {
        Task<ReturnResult<ObjectionPermitRequestListItemDto>> GetAsync(int id);

        Task<ReturnResult<ObjectionPermitRequestDetailsDto>> GetByCaseAsync(int caseId);

        Task<ReturnResult<ObjectionPermitRequestDetailsDto>> AddAsync(ObjectionPermitRequestDto objectionPermitRequestDto);

        Task<ReturnResult<ObjectionPermitRequestDto>> ReplyObjectionPermitRequest(ReplyObjectionPermitRequestDto replyObjectionPermitRequestDto);

        Task<Task> EndExpiredObjections();

    }
}