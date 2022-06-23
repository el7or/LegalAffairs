using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class BranchListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public string Parent { get; set; }

        public ICollection<int> Departments = new List<int>(); // we map it to department/branch

    }

    public class BranchDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public ICollection<int> Departments = new List<int>(); // we map it to department/branch

    }
}
