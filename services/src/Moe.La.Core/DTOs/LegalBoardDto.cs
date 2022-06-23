using Moe.La.Core.Enums;
using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class LegalBoardListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BoardHead { get; set; }
        public DateTime? UpdatedOn { get; set; } = null;
        public string UpdatedOnHigri { get; set; } = null;
        public DateTime CreatedOn { get; set; }
        public string CreatedOnHigri { get; set; }
        public KeyValuePairsDto<int> Status { get; set; }
        public string Type { get; set; }
    }

    public class LegalBoardDetailsDto : BaseDto<int>
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public Guid CreatedBy { get; set; }
        public ICollection<LegalBoardMemberDetailsDto> LegalBoardMembers { get; set; } = new List<LegalBoardMemberDetailsDto>();
    }

    public class LegalBoardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LegalBoardStatuses StatusId { get; set; }
        public LegalBoardTypes TypeId { get; set; }
        public ICollection<LegalBoradMemberDto> LegalBoardMembers { get; set; } = new List<LegalBoradMemberDto>();
    }

    public class LegalBoardSimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class LegalBoardMemoDto
    {
        public int Id { get; set; }

        public int LegalBoardId { get; set; } = 0;

        public int LegalMemoId { get; set; }
    }
}
