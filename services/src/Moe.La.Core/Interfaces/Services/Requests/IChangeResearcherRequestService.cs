using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IChangeResearcherRequestService
    {
        Task<ReturnResult<ChangeResearcherRequestListItemDto>> GetAsync(int id);

        Task<ReturnResult<ChangeResearcherRequestDetailsDto>> AddAsync(ChangeResearcherRequestDto changeResearcherRequestDto);

        Task<ReturnResult<ReplyChangeResearcherRequestDto>> AcceptChangeResearcherRequest(ReplyChangeResearcherRequestDto replyChangeResearcherRequestDto);

        Task<ReturnResult<ReplyChangeResearcherRequestDto>> RejectChangeResearcherRequest(ReplyChangeResearcherRequestDto replyChangeResearcherRequestDto);
    }
}