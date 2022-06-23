using System;

namespace Moe.La.Core.Dtos.Workflow
{
    /// <summary>
    /// Workflow instance lock Dto.
    /// </summary>
    public class WorkflowInstanceLockDto
    {
        /// <summary>
        /// Workflow instance to be locked or unlocked.
        /// </summary>
        public Guid WorkflowInstanceId { get; set; }

        /// <summary>
        /// Determine wether or the workflow instance is locked.
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// User's id who is locking the workflow instance.
        /// </summary>
        public Guid? LockedBy { get; set; }
    }
}
