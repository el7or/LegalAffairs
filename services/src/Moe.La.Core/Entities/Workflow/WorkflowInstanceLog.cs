using System;

namespace Moe.La.Core.Entities
{
    /// <summary>
    /// The workflow instance log class.
    /// </summary>
    public class WorkflowInstanceLog : BaseEntity<Guid>
    {
        /// <summary>
        /// The workflow instance id.
        /// </summary>
        public Guid WorkflowInstanceId { get; set; }

        /// <summary>
        /// The workflow instance navigation property.
        /// </summary>
        public WorkflowInstance WorkflowInstance { get; set; }

        /// <summary>
        /// The workflow step id.
        /// </summary>
        public Guid WorkflowStepId { get; set; }

        /// <summary>
        /// The workflow strp navigation property.
        /// </summary>
        public WorkflowStep WorkflowStep { get; set; }

        /// <summary>
        /// The workflow status id.
        /// </summary>
        public int WorkflowStatusId { get; set; }

        /// <summary>
        /// The workflow status navigation property.
        /// </summary>
        public WorkflowStatus WorkflowStatus { get; set; }

        /// <summary>
        /// The workflow action id.
        /// </summary>
        public Guid WorkflowActionId { get; set; }

        /// <summary>
        /// The workflow action.
        /// </summary>
        public WorkflowAction WorkflowAction { get; set; }

        //[NotMapped]
        //public WorkflowInstanceAction WorkflowInstanceAction { get; set; }

        /// <summary>
        /// The workflow instance note.
        /// </summary>
        public string WorkflowInstanceNote { get; set; }

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
    }
}
