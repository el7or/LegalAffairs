using System;

namespace Moe.La.Core.Dtos.Workflow
{
    public class WorkflowInstanceDto : BaseDto<Guid>
    {
        /// <summary>
        /// Workflow type id.
        /// </summary>
        public Guid WorkflowTypeId { get; set; }

        /// <summary>
        /// Workflow instance current step id.
        /// </summary>
        public Guid CurrentStepId { get; set; }

        /// <summary>
        /// Workflow instance current status id.
        /// </summary>
        public int CurrentStatusId { get; set; }

        /// <summary>
        /// User's id who locked the workflow instance.
        /// </summary>
        public Guid? LockedBy { get; set; }

        /// <summary>
        /// The datetime of locking the workflow instance.
        /// </summary>
        public DateTime? LockedOn { get; set; }

        /// <summary>
        /// User's id who claimed the workflow instance.
        /// </summary>
        public Guid? ClaimedBy { get; set; }

        /// <summary>
        /// The datetime of claiming the workflow instance.
        /// </summary>
        public DateTime? ClaimedOn { get; set; }

        /// <summary>
        /// User's id who is assigned to the workflow instance.
        /// </summary>
        public Guid? AssignedTo { get; set; }


        public Guid CreatedBy { get; set; }


        /// <summary>
        /// The user's id who updated it.
        /// </summary>
        public Guid? UpdatedBy { get; set; }


        /// <summary>
        /// The update datetime.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

    }
}
