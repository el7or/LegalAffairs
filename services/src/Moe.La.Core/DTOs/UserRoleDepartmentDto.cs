using System;

namespace Moe.La.Core.Dtos
{
    public class UserRoleDepartmentListItemDto
    {
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }

        public DepartmentDto Department { get; set; }
    }

    public class UserRoleDepartmentDto
    {
        public int? Id { get; set; }

        public Guid? UserId { get; set; }

        public Guid RoleId { get; set; }

        public string RoleName { get; set; }
        public string RoleNameAr { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}