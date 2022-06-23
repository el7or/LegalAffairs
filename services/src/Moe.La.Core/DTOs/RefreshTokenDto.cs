using System;

namespace Moe.La.Core.Dtos
{
    public class RefreshTokenDto
    {
        public int? Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired { get; set; }
        public DateTime? Revoked { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }

        public Guid UserId { get; set; }
    }
}
