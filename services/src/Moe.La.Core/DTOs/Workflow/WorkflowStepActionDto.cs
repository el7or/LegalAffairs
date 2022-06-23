using System;

namespace Moe.La.Core.Dtos.Workflow
{
    public class WorkflowStepActionDto : BaseDto<Guid>
    {
        /// <summary>
        /// The workflow step id.
        /// </summary>
        public Guid WorkflowStepId { get; set; }

        /// <summary>
        /// The workflow action id.
        /// </summary>
        public Guid WorkflowActionId { get; set; }

        /// <summary>
        /// The next step for the workflow instance.
        /// </summary>
        public Guid NextStepId { get; set; }

        /// <summary>
        /// The next status id.
        /// </summary>
        public int NextStatusId { get; set; }

        /// <summary>
        /// The step action description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Determines if the note should be displayed in this step action.
        /// </summary>
        public bool IsNoteVisible { get; set; }

        /// <summary>
        /// Determines if the note is required in this step action.
        /// </summary>
        public bool IsNoteRequired { get; set; }
    }

    public class WorkflowStepActionListItemDto : BaseDto<Guid>
    {
        /// <summary>
        /// Workflow step id.
        /// </summary>
        public Guid WorkflowStepId { get; set; }

        /// <summary>
        /// Workflow step name.
        /// </summary>
        public string WorkflowStepName { get; set; }

        /// <summary>
        /// Workflow action id.
        /// </summary>
        public Guid WorkflowActionId { get; set; }

        /// <summary>
        /// Workflow action name.
        /// </summary>
        public string WorkflowActionName { get; set; }

        /// <summary>
        /// Next step for the workflow instance.
        /// </summary>
        public Guid NextStepId { get; set; }

        /// <summary>
        /// Next workflow step name.
        /// </summary>
        public string NextStepName { get; set; }

        /// <summary>
        /// Next status id.
        /// </summary>
        public int NextStatusId { get; set; }

        /// <summary>
        /// Next status name.
        /// </summary>
        public string NextStatusName { get; set; }

        /// <summary>
        /// The step action description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Determines if the note should be displayed in this step action.
        /// </summary>
        public bool IsNoteVisible { get; set; }

        /// <summary>
        /// Determines if the note is required in this step action.
        /// </summary>
        public bool IsNoteRequired { get; set; }
    }
}
