using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos.Workflow
{
    public class WorkflowSearchDto
    {
        /// <summary>
        /// User's id who owns workflow instance (CreatedFor).
        /// </summary>
        public Guid? OwnerId { get; set; }

        /// <summary>
        /// User's id who locked workflow instance.
        /// </summary>
        public Guid? LockedBy { get; set; }

        /// <summary>
        /// Workflow type id.
        /// </summary>
        public Guid? WorkflowTypeId { get; set; }

        /// <summary>
        /// Workflow statuses the search will operate on.
        /// </summary>
        public IList<int> WorkflowStatuses { get; set; }
    }
}
