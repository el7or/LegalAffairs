using Moe.La.Core.Dtos;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface ICaseSupportingDocumentRequestRepository
    {
        Task<CaseSupportingDocumentRequestListItemDto> GetAsync(int id);

        Task<AttachedLetterRequestDto> GetAttachedLetterRequestAsync(int id);

        Task<CaseSupportingDocumentRequestForPrintDto> GetForPrintAsync(int id);

        Task<CaseSupportingDocumentRequestDto> AddAsync(CaseSupportingDocumentRequestDto CaseSupportingDocumentRequestDto);

        Task<AttachedLetterRequestDto> AddAttachedLetterRequestAsync(AttachedLetterRequestDto attachedLetterRequestDto);

        Task<AttachedLetterRequestDto> EditAttachedLetterRequestAsync(AttachedLetterRequestDto attachedLetterRequestDto);

        Task<CaseSupportingDocumentRequestDto> ReplyDocumentRequestAsync(ReplyCaseSupportingDocumentRequestDto replyCaseSupportingDocumentRequestDto);

        Task EditAsync(CaseSupportingDocumentRequestDto CaseSupportingDocumentRequestDto);

        Task RemoveAsync(int id);
    }
}
