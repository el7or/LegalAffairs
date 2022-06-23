using System;

namespace Moe.La.Core.Dtos.Workflow
{
    /// <summary>
    /// Workflow instance update Dto.
    /// </summary>
    public class WorkflowInstanceUpdateDto : BaseDto<Guid>
    {
        /// <summary>
        /// User's is who locked the workflow instance.
        /// </summary>
        public Guid? LockedBy { get; set; }

        /// <summary>
        /// User's id who assigned to the workflow instance.
        /// </summary>
        public Guid? AssignedTo { get; set; }

        /// <summary>
        /// User's id who claimed the workflow instance.
        /// </summary>
        public Guid ClaimedBy { get; set; }
    }
}
