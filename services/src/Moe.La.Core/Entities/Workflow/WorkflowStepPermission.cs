using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    /// <summary>
    /// The workflow step permission class.
    /// </summary>
    public class WorkflowStepPermission : BaseEntity<Guid>
    {
        /// <summary>
        /// The workflow step uniqued id.
        /// </summary>
        public Guid WorkflowStepId { get; set; }

        /// <summary>
        /// The workflow step navigation property.
        /// </summary>
        public WorkflowStep WorkflowStep { get; set; }

        /// <summary>
        /// The role id.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// The workflow instances navigatino property.
        /// </summary>
        public virtual ICollection<WorkflowInstance> WorkflowInstances { get; set; } = new List<WorkflowInstance>();
    }
}
