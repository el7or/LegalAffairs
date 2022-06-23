using Moe.La.Core.Dtos;
using Moe.La.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IObjectionPermitRequestRepository
    {
        Task<ObjectionPermitRequestListItemDto> GetAsync(int id);
        Task<ObjectionPermitRequestDetailsDto> GetByCaseAsync(int caseId);

        Task<ObjectionPermitRequestDetailsDto> AddAsync(ObjectionPermitRequestDto objectionPermitRequestDto);

        Task<ObjectionPermitRequestDto> ReplyObjectionPermitRequestAsync(ReplyObjectionPermitRequestDto replyObjectionPermitRequestDto);

        Task<List<ObjectionPermitRequest>> GetExpiredObjections();

        Task UpdateExpiredObjectionRequest(ObjectionPermitRequest _objectionRequest);

        Task<bool> CheckCaseObjectionPermitRequestAcceptedAsync(int caseId);
    }
}
