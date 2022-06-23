using Moe.La.Core.Dtos;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IAddingObjectionLegalMemoToCaseRequestRepository
    {
        Task<AddingObjectionLegalMemoToCaseRequestDto> AddAsync(AddingObjectionLegalMemoToCaseRequestDto objectionLegalMemoToCaseRequestDto);

        Task<AddingObjectionLegalMemoToCaseRequestDto> GetAsync(int Id);

        Task<AddingObjectionLegalMemoToCaseRequestDto> GetByCaseAsync(int Id);

        Task<AddingObjectionLegalMemoToCaseRequestDto> ReplyObjectionLegalMemoRequestAsync(ReplyObjectionLegalMemoRequestDto replyObjectionLegalMemoRequestDto);

        Task<bool> CheckCaseObjectionRequestAcceptedAsync(int caseId);
    }
}
