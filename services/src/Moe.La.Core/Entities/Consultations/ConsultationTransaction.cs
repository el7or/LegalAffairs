using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class ConsultationTransaction : BaseEntity<int>
    {
        public int ConsultationId { get; set; }

        public Consultation Consultation { get; set; }

        public Guid CreatedBy { get; set; }

        public AppUser CreatedByUser { get; set; }

        public string Note { get; set; }

        public ConsultationTransactionTypes TransactionType { get; set; }
    }
}
