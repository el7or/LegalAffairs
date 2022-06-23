using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    /// <summary>
    /// The workflow type class.
    /// </summary>
    public class WorkflowType : BaseEntity<Guid>
    {
        /// <summary>
        /// The workflow type Arabic name.
        /// </summary>
        public string TypeArName { get; set; }

        /// <summary>
        /// Detemines wether the workflow type is active or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Detemines the amount of time allowed for this workflow type to be locked by a given user.
        /// </summary>
        public int LockPeriod { get; set; }

        /// <summary>
        /// The workflow steps navigation property.
        /// </summary>
        public virtual ICollection<WorkflowStep> WorkflowSteps { get; set; } = new List<WorkflowStep>();

        /// <summary>
        /// The workflow actions navigation property.
        /// </summary>
        public virtual ICollection<WorkflowAction> WorkflowActions { get; set; } = new List<WorkflowAction>();

        /// <summary>
        /// The workflow instances navigation property.
        /// </summary>
        public virtual ICollection<WorkflowInstance> WorkflowInstances { get; set; } = new List<WorkflowInstance>();
    }
}
