using System;

namespace Moe.La.Core.Entities
{
    public class RequestsMoamalat : BaseEntity<int>
    {
        /// <summary>
        /// رقم الطلب.
        /// </summary>
        public int RequestId { get; set; }

        /// <summary>
        /// الطلب
        /// </summary>
        public Request Request { get; set; }

        /// <summary>
        /// رقم المعاملة
        /// </summary>
        public int MoamalatId { get; set; }

        /// <summary>
        /// المعاملة
        /// </summary>
        public Moamala Moamalat { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who created it.
        /// </summary>
        public AppUser CreatedByUser { get; set; }

        /// <summary>
        /// The user's id who updated it.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who updated it.
        /// </summary>
        public AppUser UpdatedByUser { get; set; }

        /// <summary>
        /// The update datetime.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}
