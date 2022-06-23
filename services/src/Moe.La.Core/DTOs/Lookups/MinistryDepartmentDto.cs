using System;

namespace Moe.La.Core.Dtos
{
    public class MinistryDepartmentListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public int MinistrySectorId { get; set; }

        public string MinistrySector { get; set; }

    }

    public class MinistryDepartmentDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MinistrySectorId { get; set; }

    }
}
