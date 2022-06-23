using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class RequestTransactionDto
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        public RequestStatuses RequestStatus { get; set; }

        public RequestTransactionTypes TransactionType { get; set; }

        public string Description { get; set; }
    }

    public class RequestTransactionListDto
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        public KeyValuePairsDto<int> RequestStatus { get; set; }

        public KeyValuePairsDto<int> TransactionType { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedOnHigri { get; set; } = null;
        public string CreatedOnTime { get; set; }
        public string CreatedByUser { get; set; }
        public string CreatedById { get; set; }
        //public string CreatedByRole { get; set; }
    }
}
