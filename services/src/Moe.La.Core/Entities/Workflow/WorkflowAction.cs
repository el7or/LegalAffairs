using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    /// <summary>
    /// The workflow action class.
    /// </summary>
    public class WorkflowAction : BaseEntity<Guid>
    {
        /// <summary>
        /// The workflow action Arabic name.
        /// </summary>
        public string ActionArName { get; set; }

        /// <summary>
        /// The workflow type id.
        /// </summary>
        public Guid WorkflowTypeId { get; set; }

        /// <summary>
        /// The workflow type navigation property.
        /// </summary>
        public WorkflowType WorkflowType { get; set; }

        /// <summary>
        /// The workflow instances logs navigation property.
        /// </summary>
        public virtual ICollection<WorkflowInstanceLog> WorkflowInstancesLogs { get; set; } = new List<WorkflowInstanceLog>();

        /// <summary>
        /// The workflow steps actions navigation property.
        /// </summary>
        public virtual ICollection<WorkflowStepAction> WorkflowStepsActions { get; set; } = new List<WorkflowStepAction>();
    }
}
