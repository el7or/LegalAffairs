using System;

namespace Moe.La.Core.Dtos
{
    public class DepartmentListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public int? Order { get; set; }

    }

    public class DepartmentDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Order { get; set; }
    }
}
