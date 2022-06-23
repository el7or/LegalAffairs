using System;

namespace Moe.La.Core.Dtos.Workflow
{
    public class WorkflowActionDto : BaseDto<Guid>
    {
        public WorkflowActionDto()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// The workflow action Arabic name.
        /// </summary>
        public string ActionArName { get; set; }

        /// <summary>
        /// The workflow type id.
        /// </summary>
        public Guid WorkflowTypeId { get; set; }
    }

    public class WorkflowActionListItemDto
    {
        /// <summary>
        /// The workflow action id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The workflow action Arabic name.
        /// </summary>
        public string ActionArName { get; set; }

        /// <summary>
        /// The workflow type id.
        /// </summary>
        public Guid WorkflowTypeId { get; set; }

        /// <summary>
        /// The workflow type Arabic name.
        /// </summary>
        public string WorkflowTypeArName { get; set; }

        /// <summary>
        /// The workflow type English name.
        /// </summary>
        public string WorkflowTypeEnName { get; set; }
    }
}
