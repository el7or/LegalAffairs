using System;

namespace Moe.La.Core.Entities
{
    public class RequestNote : BaseEntity<int>
    {
        public int RequestId { get; set; }

        public Request Request { get; set; }

        public string Text { get; set; }

        public Guid RoleId { get; set; }

        public AppRole Role { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who created it.
        /// </summary>
        public AppUser CreatedByUser { get; set; }
    }
}
