using System;

namespace Moe.La.Core.Dtos.Workflow
{
    /// <summary>
    /// Workflow instance log Dto.
    /// </summary>
    public class WorkflowInstanceLogDto : BaseDto<Guid>
    {
        /// <summary>
        /// Workflow instance id.
        /// </summary>
        public Guid WorkflowInstanceId { get; set; }

        /// <summary>
        /// Workflow step id.
        /// </summary>
        public Guid WorkflowStepId { get; set; }

        /// <summary>
        /// Workflow status id.
        /// </summary>
        public int WorkflowStatusId { get; set; }

        /// <summary>
        /// Workflow action id.
        /// </summary>
        public Guid WorkflowActionId { get; set; }

        /// <summary>
        /// Workflow instance note.
        /// </summary>
        public string WorkflowInstanceNote { get; set; }

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
