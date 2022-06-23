using System;

namespace Moe.La.Core.Dtos.Workflow
{
    public class WorkflowStepDto : BaseDto<Guid>
    {
        /// <summary>
        /// The workflow step Arabic name.
        /// </summary>
        public string StepArName { get; set; }

        /// <summary>
        /// The workflow type id.
        /// </summary>
        public Guid WorkflowTypeId { get; set; }

        /// <summary>
        /// The workflow step category id.
        /// </summary>
        public int WorkflowStepCategoryId { get; set; }
    }

    public class WorkflowStepListItemDto
    {
        /// <summary>
        /// The primary key.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The workflow step Arabic name.
        /// </summary>
        public string StepArName { get; set; }

        /// <summary>
        /// The workflow type id.
        /// </summary>
        public Guid WorkflowTypeId { get; set; }

        /// <summary>
        /// The workflow type Arabic name.
        /// </summary>
        public string WorkflowTypeArName { get; set; }

        /// <summary>
        /// The workflow step category id.
        /// </summary>
        public int WorkflowStepCategoryId { get; set; }

        /// <summary>
        /// The workflow step category Arabic name.
        /// </summary>
        public string WorkflowStepCategoryArName { get; set; }
    }
}
