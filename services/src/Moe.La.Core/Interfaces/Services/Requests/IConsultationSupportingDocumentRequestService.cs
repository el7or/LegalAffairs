using Moe.La.Core.Dtos;
using Moe.La.Core.Models;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IConsultationSupportingDocumentRequestService
    {
        Task<ReturnResult<ConsultationSupportingDocumentListItemDto>> GetAsync(int id);

        //Task<ReturnResult<ConsultationSupportingDocumentHistoryListItemDto>> GetHistoryAsync(int id);

        Task<ReturnResult<ConsultationSupportingDocumentRequestDto>> AddAsync(ConsultationSupportingDocumentRequestDto ConsultationSupportingDocumentDto);

        Task<ReturnResult<ConsultationSupportingDocumentRequestDto>> ReplyConsultationSupportingDocument(ReplyConsultationSupportingDocumentDto replyDocumentRequestDto);

        Task<ReturnResult<ConsultationSupportingDocumentRequestDto>> EditAsync(ConsultationSupportingDocumentRequestDto ConsultationSupportingDocumentDto);
    }
}