using System;

namespace Moe.La.Core.Dtos
{
    public class DepartmentBranchListItemDto
    {
        public int DepartmentId { get; set; }

        public int BranchId { get; set; }

        public string Department { get; set; }

        public string Branch { get; set; }
    }

    public class DepartmentBranchDto
    {
        public int DepartmentId { get; set; }

        public int BranchId { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
