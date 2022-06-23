using Moe.La.Core.Dtos;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IAddingLegalMemoToHearingRequestRepository
    {
        Task<AddingLegalMemoToHearingRequestDto> AddAsync(AddingLegalMemoToHearingRequestDto HearingLegalMemoReviewRequestDto);

        Task<AddingLegalMemoToHearingRequestDetailsDto> GetAsync(int Id);

        Task<AddingLegalMemoToHearingRequestDto> ReplyAddingMemoHearingRequestAsync(ReplyAddingLegalMemoToHearingRequestDto replyAddingLegalMemoToHearingRequest);
    }
}
