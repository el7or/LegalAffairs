using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public string NameAr { get; set; }

        public int? Priority { get; set; }

        /// <summary>
        /// امكانية ان  يتم توزيع صاحب  هذا الدور على الادارات التخصصية   
        /// </summary>
        public bool IsDistributable { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
    }
}
