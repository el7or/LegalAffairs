using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class ProvinceListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public ICollection<string> Cities { get; set; } = new List<string>();

    }

    public class ProvinceDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
