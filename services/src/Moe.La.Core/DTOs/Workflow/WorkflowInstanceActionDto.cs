using System;

namespace Moe.La.Core.Dtos.Workflow
{
    /// <summary>
    /// Represent an action related to a workflow instance.
    /// </summary>
    public class WorkflowInstanceActionDto : BaseDto<Guid>
    {
        /// <summary>
        /// Workflow action Arabic name.
        /// </summary>
        public string ActionArName { get; set; }

        /// <summary>
        /// Workflow type id.
        /// </summary>
        public Guid WorkflowTypeId { get; set; }

        /// <summary>
        /// Display name for the action when associated with a step.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Define wether the note field is visible or not.
        /// </summary>
        public bool IsNoteVisible { get; set; }

        /// <summary>
        /// Define wether the note field is required or not.
        /// </summary>
        public bool IsNoteRequired { get; set; }
    }
}
