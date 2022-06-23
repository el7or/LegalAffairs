using System;

namespace Moe.La.Core.Dtos
{
    public class RoleListItemDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NameAr { get; set; }

        public int? Priority { get; set; }
        public bool IsDistributable { get; set; }
    }

    public class RoleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NameAr { get; set; }

        public int? Priority { get; set; }
    }
}
