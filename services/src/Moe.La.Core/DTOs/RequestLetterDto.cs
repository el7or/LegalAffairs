using Moe.La.Core.Enums;

namespace Moe.La.Core.Dtos
{
    public class RequestLetterDto
    {
        public int RequestId { get; set; }

        public string Text { get; set; }

        public RequestStatuses RequestStatus { get; set; }

    }
    public class RequestLetterDetailsDto
    {
        public int? RequestId { get; set; }

        public string Text { get; set; }
    }
    public class ReplaceDepartmentDto
    {
        public string Contnet { get; set; }
        public string DepartmentName { get; set; }
    }
}
