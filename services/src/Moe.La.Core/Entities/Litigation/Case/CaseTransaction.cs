using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class CaseTransaction : BaseEntity<int>
    {
        public int CaseId { get; set; }
        public Case Case { get; set; }
        public Guid CreatedBy { get; set; }
        public AppUser CreatedByUser { get; set; }
        public string Note { get; set; }

        public CaseTransactionTypes TransactionType { get; set; }


    }
}
