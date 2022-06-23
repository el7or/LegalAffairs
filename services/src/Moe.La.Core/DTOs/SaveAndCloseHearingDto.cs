namespace Moe.La.Core.Dtos
{
    public class HearingCloseCreateDto
    {
        public int CurrentHearingId { get; set; }

        public string ClosingReport { get; set; }

        public HearingDto NewHearing { get; set; }
    }
}
