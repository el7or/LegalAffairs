using Moe.La.Core.Enums;
using System;

namespace Moe.La.Core.Dtos
{
    public class LegalMemosHistoryListItemDto
    {
        public int Id { get; set; }

        public int LegalMemoId { get; set; }

        public string Name { get; set; }

        public LegalMemoStatuses Status { get; set; }

        public DateTime UpdatedOn { get; set; }

        public bool? IsRead { get; set; }
    }

    public class LegalMemosHistoryDto
    {
        public int Id { get; set; }

        public int LegalMemoId { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public string Name { get; set; }

        public LegalMemoStatuses Status { get; set; }

        public string Text { get; set; }

        public int ReviewNumber { get; set; }

        public bool? IsRead { get; set; }

        public int? InitialCaseId { get; set; }
    }
}
