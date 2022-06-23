using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IAddingObjectionLegalMemoToCaseRequestService
    {
        Task<ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>> AddAsync(AddingObjectionLegalMemoToCaseRequestDto addingObjectionLegalMemoToCaseRequestDto);

        Task<ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>> GetAsync(int Id);

        Task<ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>> GetByCaseAsync(int caseId);

        Task<ReturnResult<AddingObjectionLegalMemoToCaseRequestDto>> ReplyObjectionLegalMemoRequest(ReplyObjectionLegalMemoRequestDto replyObjectionLegalMemoRequestDto);
    }
}