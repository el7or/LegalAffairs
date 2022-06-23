using Moe.La.Core.Dtos;
using Moe.La.Core.Enums;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IChangeResearcherRequestRepository
    {
        Task<ChangeResearcherRequestListItemDto> GetAsync(int id);

        Task<ChangeResearcherRequestDetailsDto> AddAsync(ChangeResearcherRequestDto changeResearcherRequestDto);

        Task EditAsync(ChangeResearcherRequestDto changeResearcherRequestDto);

        Task<ChangeResearcherRequestListItemDto> ReplyChangeResearcherRequestAsync(ReplyChangeResearcherRequestDto replyChangeResearcherRequestDto, RequestStatuses status);
    }
}
