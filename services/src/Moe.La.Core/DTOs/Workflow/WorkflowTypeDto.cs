using System;
using System.Collections.Generic;

namespace Moe.La.Core.Dtos.Workflow
{
    public class WorkflowTypeDto : BaseDto<Guid>
    {
        /// <summary>
        /// The workflow type Arabic name.
        /// </summary>
        public string TypeArName { get; set; }

        /// <summary>
        /// Detemines wether the workflow type is active or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Detemines the amount of time allowed for this workflow type to be locked by a given user.
        /// </summary>
        public int LockPeriod { get; set; }
    }

    public class WorkflowTypeListItemDto : BaseDto<Guid>
    {
        /// <summary>
        /// The workflow type Arabic name.
        /// </summary>
        public string TypeArName { get; set; }

        /// <summary>
        /// Detemines wether the workflow type is active or not.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Detemines the amount of time allowed for this workflow type to be locked by a given user.
        /// </summary>
        public int LockPeriod { get; set; }
    }

    public class WorkflowTypeViewDto
    {
        public WorkflowTypeListItemDto WorkflowType { get; set; }

        public IList<WorkflowActionListItemDto> WorkflowActions { get; set; }

        public IList<WorkflowStepListItemDto> WorkflowSteps { get; set; }

        public IList<WorkflowStepActionListItemDto> WorkflowStepsActions { get; set; }
    }
}
