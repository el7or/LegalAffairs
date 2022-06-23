using Microsoft.AspNetCore.Identity;
using System;

namespace Moe.La.Core.Entities
{
    public class AppRoleClaim : IdentityRoleClaim<Guid>
    {
        public string NameAr { get; set; }

        public string Description { get; set; }
    }
}
