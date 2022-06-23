using System;

namespace Moe.La.Core.Dtos
{
    public class WorkItemTypeListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public string Roles { get; set; }

        public KeyValuePairsDto<int> Department { get; set; }
    }

    public class WorkItemTypeDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public string RolesIds { get; set; }
    }
}
