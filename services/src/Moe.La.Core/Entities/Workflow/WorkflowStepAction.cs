using System;

namespace Moe.La.Core.Entities
{
    /// <summary>
    /// The workflow step action class.
    /// </summary>
    public class WorkflowStepAction : BaseEntity<Guid>
    {
        /// <summary>
        /// The workflow step id.
        /// </summary>
        public Guid WorkflowStepId { get; set; }

        /// <summary>
        /// The workflow step navigation property.
        /// </summary>
        public WorkflowStep WorkflowStep { get; set; }

        /// <summary>
        /// The workflow action id.
        /// </summary>
        public Guid WorkflowActionId { get; set; }

        /// <summary>
        /// The workflow action navigation property.
        /// </summary>
        public WorkflowAction WorkflowAction { get; set; }

        /// <summary>
        /// The next step for the workflow instance.
        /// </summary>
        public Guid NextStepId { get; set; }

        /// <summary>
        /// The next step for the workflow instance navigation property.
        /// </summary>
        public WorkflowStep NextStep { get; set; }

        /// <summary>
        /// The next status id.
        /// </summary>
        public int NextStatusId { get; set; }

        /// <summary>
        /// The next workflow status for the workflow instance navigation property.
        /// </summary>
        public WorkflowStatus NextStatus { get; set; }

        /// <summary>
        /// The step action description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Determines wether the note field is visible or not.
        /// </summary>
        public bool IsNoteVisible { get; set; }

        /// <summary>
        /// Determines wether the note field is required or not.
        /// </summary>
        public bool IsNoteRequired { get; set; }
    }
}
