using System;

namespace Moe.La.Core.Entities
{
    public class CaseGrounds : BaseEntity<int>
    {
        public int CaseId { get; set; }

        public Case Case { get; set; }

        public Guid CreatedBy { get; set; }

        public AppUser CreatedByUser { get; set; }

        public Guid? UpdatedBy { get; set; }

        public AppUser UpdatedByUser { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string Text { get; set; }
    }
}
