using System;

namespace Moe.La.Core.Dtos
{
    public class CaseResearchersDto : BaseDto<int>
    {
        public int CaseId { get; set; }

        public Guid ResearcherId { get; set; }

        public string Note { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? CreatedByRole { get; set; }

        public int? CreatedByBranchId { get; set; }
    }
}