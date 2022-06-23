using System;

namespace Moe.La.Core.Dtos
{
    public class RequestNoteListItemDto
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        public string Text { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedOnHigri { get; set; }

        public string CreatedBy { get; set; }

        public RoleDto Role { get; set; }
    }

    public class RequestNoteDto
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        public string Text { get; set; }
    }
}
