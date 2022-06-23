using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos.Consultations
{
    public class ConsultationTransactionDto
    {
        public int Id { get; set; }

        public int ConsultationId { get; set; }

        public ConsultationStatus ConsultationStatus { get; set; }

        public ConsultationTransactionTypes TransactionType { get; set; }
        public AppUser CreatedByUser { get; set; }

        public DateTime CreatedOn { get; set; }
        public string Note { get; set; }
    }
    public class ConsultationTransactionListDto
    {
        public int Id { get; set; }

        public int ConsultationId { get; set; }

        public KeyValuePairsDto<int> ConsultationStatus { get; set; }

        public KeyValuePairsDto<int> TransactionType { get; set; }

        public string Note { get; set; }

        public string CreatedByUser { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnHigri { get; set; } = null;

        public string CreatedOnTime { get; set; }

        //public string CreatedByRole { get; set; }

    }
}
