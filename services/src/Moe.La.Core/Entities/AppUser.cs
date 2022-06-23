using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        public string LastName { get; set; }

        public string Picture { get; set; }

        public Byte[] Signature { get; set; }

        public int? BranchId { get; set; }

        public Branch Branch { get; set; }

        public int? JobTitleId { get; set; }

        public JobTitle JobTitle { get; set; }

        public string IdentityNumber { get; set; }

        public string EmployeeNo { get; set; }

        public int? ExtensionNumber { get; set; }

        public string Token { get; set; }

        public DateTime? TokenCreateDate { get; set; }

        public bool Enabled { get; set; }

        public string Nationality { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// The creation datetime.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// The user's id who updated it.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        // <summary>
        /// The update datetime.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Logical delete, if true don't show it.
        /// </summary>
        public bool IsDeleted { get; set; }

        //public ICollection<AppUser> ManagerEmployees { get; set; } = new List<AppUser>();

        public ICollection<ResearcherConsultant> Researchers { get; set; } = new List<ResearcherConsultant>();

        public ICollection<ResearcherConsultant> Consultants { get; set; } = new List<ResearcherConsultant>();

        public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();

        [JsonIgnore]
        public ICollection<AppUserClaim> UserClaims { get; set; } = new List<AppUserClaim>();

        //for refresh token which send with normal access token
        [JsonIgnore]
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
