namespace Moe.La.Core.Dtos
{
    public class HearingLegalMemoDetailsDto
    {
        public HearingDto Hearing { get; set; }

        public LegalMemoDetailsDto LegalMemo { get; set; }

        public RequestListItemDto Request { get; set; }

    }

    public class HearingLegalMemoDto
    {

        public int Id { get; set; }

        public RequestDto Request { get; set; }

        public int HearingId { get; set; }

        public int LegalMemoId { get; set; }
    }
}
