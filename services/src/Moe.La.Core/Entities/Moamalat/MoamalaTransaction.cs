using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class MoamalaTransaction : BaseEntity<int>
    {
        public int MoamalaId { get; set; }
        public Moamala Moamala { get; set; }
        public Guid CreatedBy { get; set; }
        public AppUser CreatedByUser { get; set; }
        public string Note { get; set; }

        public MoamalaTransactionTypes TransactionType { get; set; }
    }
}
