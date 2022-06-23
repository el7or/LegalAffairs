using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class AddingLegalMemoToHearingRequestListItemDto
    {
        public HearingDetailsDto Hearing { get; set; }

        public LegalMemoDetailsDto LegalMemo { get; set; }

        public RequestListItemDto Request { get; set; }

        public string ReplyNote { get; set; }

        public string ReplyDateHigri { get; set; }
        public DateTime ReplyDate { get; set; }
    }
    public class AddingLegalMemoToHearingRequestDetailsDto
    {
        public HearingDetailsDto Hearing { get; set; }

        public LegalMemoDetailsDto LegalMemo { get; set; }

        public RequestListItemDto Request { get; set; }
    }

    public class AddingLegalMemoToHearingRequestDto
    {
        public int Id { get; set; }

        public RequestDto Request { get; set; }

        public int HearingId { get; set; }

        public int LegalMemoId { get; set; }
        public string ReplyNote { get; set; }
    }
    public class ReplyAddingLegalMemoToHearingRequestDto
    {
        public int Id { get; set; }

        public string ReplyNote { get; set; }

        public RequestStatuses RequestStatus { get; set; }
    }
    public class HearingAddedLegalMemoRequestDto
    {

        public string LegalMemoName { get; set; }

        public string AddRequestDate { get; set; }

        public string ApproveRequestDate { get; set; }

        public string ApprovedBy { get; set; }

        public KeyValuePairsDto<int> RequestStatus { get; set; }
    }


}
