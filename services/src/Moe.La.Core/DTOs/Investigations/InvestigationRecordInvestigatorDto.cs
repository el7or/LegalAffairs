using System;

namespace Moe.La.Core.Dtos
{
    public class InvestigationRecordInvestigatorListItemDto : BaseDto<int>
    {

    }

    public class InvestigationRecordInvestigatorDetailsDto : BaseDto<int>
    {

    }

    public class InvestigationRecordInvestigatorDto
    {
        public int? Id { get; set; } = 0;

        public int InvestigationRecordId { get; set; }

        public Guid InvestigatorId { get; set; }
    }

}
