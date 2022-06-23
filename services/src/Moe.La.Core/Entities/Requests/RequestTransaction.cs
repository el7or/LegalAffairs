using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Entities
{
    public class RequestTransaction : BaseEntity<int>
    {
        public int RequestId { get; set; }
        public Request Request { get; set; }

        public RequestStatuses RequestStatus { get; set; }

        public RequestTransactionTypes TransactionType { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who created it.
        /// </summary>
        public AppUser CreatedByUser { get; set; }

        public string Description { get; set; }

    }
}
