using Moe.La.Core.Constants;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Models
{
    /// <summary>
    /// Used to capture the current logged in user information.
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// User's ID.
        /// </summary>
        public Guid UserId { get; set; } = ApplicationConstants.SystemAdministratorId;

        /// <summary>
        /// Username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User's identity number.
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// User's computer host name.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// User's IP address.
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// User's general management.
        /// </summary>
        public int BranchId { get; set; }

        /// <summary>
        /// User's roles.
        /// </summary>
        public IList<string> Roles { get; set; } = new List<string>();

        /// <summary>
        /// User's departments within the General Management.
        /// </summary>
        public IList<int> Departments { get; set; } = new List<int>();

        public IList<string> Permissions { get; set; } = new List<string>();

    }
}
