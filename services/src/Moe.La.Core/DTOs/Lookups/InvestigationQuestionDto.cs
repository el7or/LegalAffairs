using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class InvestigationQuestionListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Question { get; set; }

        public KeyValuePairsDto<int> Status { get; set; }
    }

    public class InvestigationQuestionDto
    {
        public int? Id { get; set; }

        public string Question { get; set; }

        public InvestigationQuestionStatuses Status { get; set; }
    }

    public class InvestigationQuestionChangeStatusDto
    {
        public int Id { get; set; }

        public InvestigationQuestionStatuses Status { get; set; }
    }
}
