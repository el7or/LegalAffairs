using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class LegalBoradMemberDto
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public LegalBoardMembershipTypes MembershipType { get; set; }
        public bool IsActive { get; set; }
    }

    public class LegalBoardMemberDetailsDto : BaseDto<int>
    {
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public int LegalBoardId { get; set; }
        public KeyValuePairsDto<int> MembershipType { get; set; }
        public DateTime? StartDate { get; set; } = null;
        public string StartDateHigri { get; set; } = null;
        public bool IsActive { get; set; }
    }

    public class LegalBoardMemberHistoryDto : BaseDto<int>
    {
        public Guid UserId { get; set; }
        public int LegalBoardId { get; set; }
        public string MembershipType { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateHigri { get; set; } = null;
        public DateTime? EndDate { get; set; }
        public string EndDateHigri { get; set; } = null;
        public bool IsActive { get; set; }
    }
}
