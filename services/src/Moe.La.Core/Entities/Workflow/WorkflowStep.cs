using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    /// <summary>
    /// The workflow step.
    /// </summary>
    public class WorkflowStep : BaseEntity<Guid>
    {
        /// <summary>
        /// The workflow step Arabic name.
        /// </summary>
        public string StepArName { get; set; }

        /// <summary>
        /// The workflow type id.
        /// </summary>
        public Guid WorkflowTypeId { get; set; }

        /// <summary>
        /// The workflow type navigation property.
        /// </summary>
        public WorkflowType WorkflowType { get; set; }

        /// <summary>
        /// The workflow step category id.
        /// </summary>
        public int WorkflowStepCategoryId { get; set; }

        /// <summary>
        /// The workflow category navigation property.
        /// </summary>
        public WorkflowStepCategory WorkflowStepCategory { get; set; }

        /// <summary>
        /// The workflow instances navigation property.
        /// </summary>
        public virtual ICollection<WorkflowInstance> WorkflowInstances { get; set; } = new List<WorkflowInstance>();

        /// <summary>
        /// The workflow instances logs navigation property.
        /// </summary>
        public virtual ICollection<WorkflowInstanceLog> WorkflowInstancesLogs { get; set; } = new List<WorkflowInstanceLog>();

        /// <summary>
        /// The workflow current steps navigation property.
        /// </summary>
        public virtual ICollection<WorkflowStepAction> WorkflowCurrentSteps { get; set; } = new List<WorkflowStepAction>();

        /// <summary>
        /// The workflow next steps navigation property.
        /// </summary>
        public virtual ICollection<WorkflowStepAction> WorkflowNextSteps { get; set; } = new List<WorkflowStepAction>();

        /// <summary>
        /// The workflow steps permissions navigation property.
        /// </summary>
        public virtual ICollection<WorkflowStepPermission> WorkflowStepsPermissions { get; set; } = new List<WorkflowStepPermission>();
    }
}
