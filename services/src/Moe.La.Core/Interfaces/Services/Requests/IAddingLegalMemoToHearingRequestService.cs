using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IAddingLegalMemoToHearingRequestService
    {
        Task<ReturnResult<AddingLegalMemoToHearingRequestDto>> AddAsync(AddingLegalMemoToHearingRequestDto addingLegalMemoToHearingRequestDto);

        Task<ReturnResult<AddingLegalMemoToHearingRequestDetailsDto>> GetAsync(int Id);

        Task<ReturnResult<ReplyAddingLegalMemoToHearingRequestDto>> ReplyAddingMemoHearingRequest(ReplyAddingLegalMemoToHearingRequestDto replyAddingLegalMemoToHearingRequest);
    }
}