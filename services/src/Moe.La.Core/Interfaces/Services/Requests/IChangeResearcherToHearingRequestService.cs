using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IChangeResearcherToHearingRequestService
    {
        Task<ReturnResult<ChangeResearcherToHearingRequestListItemDto>> GetAsync(int id);

        Task<ReturnResult<ChangeResearcherToHearingRequestDetailsDto>> AddAsync(ChangeResearcherToHearingRequestDto changeResearcherToHearingRequestDto);

        Task<ReturnResult<ReplyChangeResearcherToHearingRequestDto>> AcceptChangeResearcherToHearingRequest(ReplyChangeResearcherToHearingRequestDto replyChangeResearcherToHearingRequestDto);

        Task<ReturnResult<ReplyChangeResearcherToHearingRequestDto>> RejectChangeResearcherToHearingRequest(ReplyChangeResearcherToHearingRequestDto replyChangeResearcherToHearingRequestDto);

        Task<Task> CanceledChangeResearcherToHearingRequests();
    }
}