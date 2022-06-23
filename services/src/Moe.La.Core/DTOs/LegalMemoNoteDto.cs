using System;

namespace Moe.La.Core.Dtos
{
    public class LegalMemoNoteListItemDto
    {
        public int Id { get; set; }

        public int LegalMemoId { get; set; }

        public int ReviewNumber { get; set; }

        public string Text { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnHigri { get; set; }

        public string CreationTime { get; set; }

        public KeyValuePairsDto<Guid> CreatedBy { get; set; }

        public bool IsClosed { get; set; }

        public string BoardName { get; set; }
    }

    public class LegalMemoNoteDto
    {
        public int Id { get; set; }

        public int LegalMemoId { get; set; }

        public int? LegalBoardId { get; set; }

        public int ReviewNumber { get; set; }

        public string Text { get; set; }

        public bool IsClosed { get; set; }
    }
}
