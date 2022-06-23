using System;

namespace Moe.La.Core.Dtos
{
    public class InvestigationRecordPartyTypeListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }
    }

    public class InvestigationRecordPartyTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
