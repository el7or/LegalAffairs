using System;

namespace Moe.La.Core.Dtos.Workflow
{
    /// <summary>
    /// Workflow initiator Dto.
    /// </summary>
    public class WorkflowInitiatorDto
    {
        /// <summary>
        /// Workflow type id.
        /// </summary>
        public Guid WorkflowTypeId { get; set; }

        /// <summary>
        /// Workflow action id.
        /// </summary>
        public Guid WorkflowActionId { get; set; }

        /// <summary>
        /// <summary>
        /// The user's id who created it.
        /// </summary>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// The creation datetime.
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
