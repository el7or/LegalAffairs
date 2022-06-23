using System;

namespace Moe.La.Core.Dtos
{
    public class SubWorkItemTypeListItemDto
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public KeyValuePairsDto<int> WorkItemType { get; set; }

    }

    public class SubWorkItemTypeDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public int WorkItemTypeId { get; set; }

    }
}
