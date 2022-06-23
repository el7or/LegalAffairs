using System;

namespace Moe.La.Core.Entities
{
    public class RefreshToken : BaseEntity<int>
    {
        public string Token { get; set; }

        public DateTime? Expires { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;

        public DateTime? Revoked { get; set; }

        public string ReplacedByToken { get; set; }

        public bool IsActive => Revoked == null && !IsExpired;

        public Guid? UserId { get; set; }

        public AppUser User { get; set; }
    }
}