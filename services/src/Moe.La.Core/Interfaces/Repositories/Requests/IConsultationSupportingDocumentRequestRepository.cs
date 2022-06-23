using Moe.La.Core.Dtos;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IConsultationSupportingDocumentRequestRepository
    {
        Task<ConsultationSupportingDocumentListItemDto> GetAsync(int id);

        Task<ConsultationSupportingDocumentRequestDto> AddAsync(ConsultationSupportingDocumentRequestDto ConsultationSupportingDocumentDto);

        Task<ConsultationSupportingDocumentRequestDto> ReplyConsultationSupportingDocumentAsync(ReplyConsultationSupportingDocumentDto replyConsultationSupportingDocumentDto);

        Task EditAsync(ConsultationSupportingDocumentRequestDto ConsultationSupportingDocumentDto);

    }
}
