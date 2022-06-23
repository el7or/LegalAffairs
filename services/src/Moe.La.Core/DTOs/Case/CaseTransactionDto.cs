using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class CaseTransactionDetailsDto
    {
        public string CreatedByUser { get; set; }
        //public string CreatedByRole { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnHigri { get; set; } = null;
        public string CreatedOnTime { get; set; }
        public string TransactionType { get; set; }


    }

    public class CaseTransactionDto
    {
        public int CaseId { get; set; }
        public string Note { get; set; }
        public CaseTransactionTypes TransactionType { get; set; }

    }
}