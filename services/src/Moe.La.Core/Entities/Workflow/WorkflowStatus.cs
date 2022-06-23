using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    /// <summary>
    /// Specifies the workflow instance statuses during its lifetime.
    /// </summary>
    public enum WorkflowStatuses
    {
        Uncompleted = 1,
        New = 2,
        UnderProcess = 3,
        Returned = 4,
        Canceled = 5,
        Finished = 6
    }

    /// <summary>
    /// The workflow status class.
    /// </summary>
    public class WorkflowStatus : BaseEntity<int>
    {
        /// <summary>
        /// The workflow status Arabic name.
        /// </summary>
        public string StatusArName { get; set; }

        /// <summary>
        /// The workflow instances navigation property.
        /// </summary>
        public virtual ICollection<WorkflowInstance> WorkflowInstances { get; set; } = new List<WorkflowInstance>();

        /// <summary>
        /// The workflow instances logs navigation propery.
        /// </summary>
        public virtual ICollection<WorkflowInstanceLog> WorkflowInstancesLogs { get; set; } = new List<WorkflowInstanceLog>();

        /// <summary>
        /// The workflow next statuses navigation property.
        /// </summary>
        public virtual ICollection<WorkflowStepAction> WorkflowNextStatuses { get; set; } = new List<WorkflowStepAction>();
    }
}
