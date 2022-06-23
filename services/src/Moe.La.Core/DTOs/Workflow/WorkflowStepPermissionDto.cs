using System;

namespace Moe.La.Core.Dtos.Workflow
{
    public class WorkflowStepPermissionDto : BaseDto<Guid>
    {
        /// <summary>
        /// The workflow step uniqued id.
        /// </summary>
        public Guid WorkflowStepId { get; set; }

        /// <summary>
        /// The role id.
        /// </summary>
        public int RoleId { get; set; }
    }
}
