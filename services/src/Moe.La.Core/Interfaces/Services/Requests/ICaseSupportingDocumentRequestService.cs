using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface ICaseSupportingDocumentRequestService
    {
        Task<ReturnResult<CaseSupportingDocumentRequestListItemDto>> GetAsync(int id);

        Task<ReturnResult<AttachedLetterRequestDto>> GetAttachedLetterRequestAsync(int id);

        Task<CaseSupportingDocumentRequestForPrintDto> GetForPrintAsync(int id);

        Task<ReturnResult<CaseSupportingDocumentRequestDto>> AddAsync(CaseSupportingDocumentRequestDto CaseSupportingDocumentRequestDto);

        Task<ReturnResult<AttachedLetterRequestDto>> AddAttachedLetterRequestAsync(AttachedLetterRequestDto attachedLetterRequestDto);

        Task<ReturnResult<AttachedLetterRequestDto>> EditAttachedLetterRequestAsync(AttachedLetterRequestDto attachedLetterRequestDto);

        Task<ReturnResult<CaseSupportingDocumentRequestDto>> ReplyCaseSupportingDocumentRequest(ReplyCaseSupportingDocumentRequestDto replyDocumentRequestDto);

        Task<ReturnResult<CaseSupportingDocumentRequestDto>> EditAsync(CaseSupportingDocumentRequestDto CaseSupportingDocumentRequestDto);
    }
}