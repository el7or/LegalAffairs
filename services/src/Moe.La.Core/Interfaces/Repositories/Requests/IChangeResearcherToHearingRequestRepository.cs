using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IChangeResearcherToHearingRequestRepository
    {
        Task<ChangeResearcherToHearingRequestListItemDto> GetAsync(int id);

        Task<ChangeResearcherToHearingRequestDetailsDto> AddAsync(ChangeResearcherToHearingRequestDto changeResearcherToHearingRequestDto);

        Task<ChangeResearcherToHearingRequestListItemDto> ReplyChangeResearcherToHearingRequestAsync(
            ReplyChangeResearcherToHearingRequestDto replyChangeResearcherToHearingRequestDto,
            RequestStatuses status);

        Task<bool> IsMoreRequestsForSameHearing(ChangeResearcherToHearingRequestDto changeResearcherToHearingRequestDto);

        Task CanceledChangeResearcherToHearingRequests();
    }
}
