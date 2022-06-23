using System;

namespace Moe.La.Core.Entities
{
    public class MoamalaNotification : BaseEntity<int>
    {
        public int MoamalaId { get; set; }
        public Moamala Moamala { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public bool IsRead { get; set; }
    }
}
