using Moe.La.Core.Enums;

namespace Moe.La.Core.Dtos
{
    public class ConsultationSupportingDocumentListItemDto
    {
        public int ConsultationId { get; set; }

        public int RequestId { get; set; }

        public RequestListItemDto Request { get; set; }

    }

    public class ConsultationSupportingDocumentRequestDto
    {
        public int ConsultationId { get; set; }

        public int MoamalaId { get; set; }

        public int RequestId { get; set; }

        public RequestDto Request { get; set; }

    }

    public class ReplyConsultationSupportingDocumentDto
    {
        public int ConsultationId { get; set; }

        public int RequestId { get; set; }

        public RequestStatuses RequestStatus { get; set; }


    }

}
