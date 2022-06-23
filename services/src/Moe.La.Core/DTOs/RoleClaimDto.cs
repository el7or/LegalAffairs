using System;

namespace Moe.La.Core.Dtos
{
    public class RoleClaimListItemDto
    {
        public int Id { get; set; }

        public Guid RoleId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public string NameAr { get; set; }

        public string Description { get; set; }
    }

    public class RoleClaimDto
    {
        public string RoleName { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }

    public class ClaimDto
    {
        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}
