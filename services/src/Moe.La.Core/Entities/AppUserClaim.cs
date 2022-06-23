using Microsoft.AspNetCore.Identity;
using System;

namespace Moe.La.Core.Entities
{
    public class AppUserClaim : IdentityUserClaim<Guid>
    {
        public virtual AppUser User { get; set; }
    }
}
