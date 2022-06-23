using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class MoamalaTransactionDetailsDto
    {
        public string CreatedByUser { get; set; }
        //public string CreatedByRole { get; set; }
        public string Note { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnHigri { get; set; }
        public string TransactionType { get; set; }


    }

    public class MoamalaTransactionDto
    {
        public int MoamalaId { get; set; }
        public string Note { get; set; }
        public MoamalaTransactionTypes TransactionType { get; set; }

    }
}