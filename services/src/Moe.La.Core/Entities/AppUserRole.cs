using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class AppUserRole : IdentityUserRole<Guid>
    {
        public virtual AppUser User { get; set; }

        public virtual AppRole Role { get; set; }

        public ICollection<UserRoleDepartment> UserRoleDepartmets { get; set; } = new List<UserRoleDepartment>();
    }
}
