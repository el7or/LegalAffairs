using System;

namespace Moe.La.Core.Dtos
{
    public class JobTitleListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }
    }

    public class JobTitleDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
