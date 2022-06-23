using System;

namespace Moe.La.Core.Dtos
{
    public class MinistrySectorListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }
    }

    public class MinistrySectorDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
