using System;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    /// <summary>
    /// The workflow instance class.
    /// </summary>
    public class WorkflowInstance : BaseEntity<Guid>
    {
        /// <summary>
        /// The workflow type id.
        /// </summary>
        public Guid WorkflowTypeId { get; set; }

        /// <summary>
        /// The workflow type navigation property.
        /// </summary>
        public WorkflowType WorkflowType { get; set; }

        /// <summary>
        /// The current step id where the this workflow instance is in.
        /// </summary>
        public Guid CurrentStepId { get; set; }

        /// <summary>
        /// The current step navigation property.
        /// </summary>
        public WorkflowStep CurrentStep { get; set; }

        /// <summary>
        /// The current status id.
        /// </summary>
        public int CurrentStatusId { get; set; }

        /// <summary>
        /// The current status navigation property.
        /// </summary>
        public WorkflowStatus CurrentStatus { get; set; }

        /// <summary>
        /// The current step permission id for this workflow instance.
        /// </summary>
        public Guid? WorkflowStepPermissionId { get; set; }

        /// <summary>
        /// The workflow step permission navigation property.
        /// </summary>
        public WorkflowStepPermission WorkflowStepPermission { get; set; }

        /// <summary>
        /// The locked on datetime.
        /// </summary>
        public DateTime? LockedOn { get; set; }

        /// <summary>
        /// The user's id who is locked by
        /// </summary>
        public Guid? LockedBy { get; set; }

        /// <summary>
        /// The claim on datetime.
        /// </summary>
        public DateTime? ClaimedOn { get; set; }

        /// <summary>
        /// The user's id who is claimed by.
        /// </summary>
        public Guid? ClaimedBy { get; set; }

        /// <summary>
        /// the user's id who is assigned to.
        /// </summary>
        public Guid? AssignedTo { get; set; }

        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who created it.
        /// </summary>
        public AppUser CreatedByUser { get; set; }

        /// <summary>
        /// The user's id who updated it.
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Navigation property to the user who updated it.
        /// </summary>
        public AppUser UpdatedByUser { get; set; }

        /// <summary>
        /// The update datetime.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// The workflow instances logs navigation property.
        /// </summary>
        public virtual ICollection<WorkflowInstanceLog> WorkflowInstancesLogs { get; set; } = new List<WorkflowInstanceLog>();
    }
}
