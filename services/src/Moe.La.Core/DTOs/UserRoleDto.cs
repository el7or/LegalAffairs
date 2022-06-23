using System.Collections.Generic;

namespace Moe.La.Core.Dtos
{
    public class UserRoleDto
    {
        public ICollection<UserRoleDepartmentListItemDto> UserRoleDepartmets { get; set; } = new List<UserRoleDepartmentListItemDto>();
    }
}
