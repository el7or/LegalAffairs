using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    public class WorkItemType
    {
        /// <summary>
        /// The primary key.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The creation datetime.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        public string Name { get; set; }

        public string RolesIds { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public ICollection<SubWorkItemType> SubWorkItemTypes { get; set; }
    }
}
