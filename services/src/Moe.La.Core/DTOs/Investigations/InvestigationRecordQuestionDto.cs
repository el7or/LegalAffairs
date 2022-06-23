namespace Moe.La.Core.Dtos
{
    public class InvestigationRecordQuestionListItemDto : BaseDto<int>
    {

    }

    public class InvestigationRecordQuestionDetailsDto : BaseDto<int>
    {

    }

    public class InvestigationRecordQuestionDto
    {
        public int? Id { get; set; }

        public int? QuestionId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public InvestigationRecordPartyDto AssignedTo { get; set; }

    }


}
