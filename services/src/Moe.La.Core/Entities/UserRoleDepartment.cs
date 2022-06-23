using System;

namespace Moe.La.Core.Entities
{
    public class UserRoleDepartment : BaseEntity<int>
    {
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

        public AppUserRole UserRole { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public Guid CreatedBy { get; set; }

        public AppUser CreatedByUser { get; set; }
    }
}
