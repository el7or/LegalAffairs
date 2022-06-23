using System;

namespace Moe.La.Core.Dtos.Workflow
{
    public class WorkflowProcessDto
    {
        /// <summary>
        /// The workflow instance id.
        /// </summary>
        public Guid WorkflowInstanceId { get; set; }

        /// <summary>
        /// The workflow action id.
        /// </summary>
        public Guid WorkflowActionId { get; set; }

        /// <summary>
        /// The workflow instance processing note.
        /// </summary>
        public string ProcessNote { get; set; }

        /// <summary>
        /// To whom the workflow instance will be assigned.
        /// </summary>
        public Guid? AssignedTo { get; set; }

        /// <summary>
        /// Personnel for whom the workflow instance is created.
        /// </summary>
        public Guid CreatedFor { get; set; }
    }
}
