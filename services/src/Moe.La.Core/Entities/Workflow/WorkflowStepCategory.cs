using Moe.La.Common.Extensions;
using Moe.La.Common.Resources.Common;
using System.Collections.Generic;

namespace Moe.La.Core.Entities
{
    /// <summary>
    /// Specifies the workflow step categories.
    /// </summary>
    public enum WorkflowStepsCategories : int
    {
        [LocalizedDescription("Initiator", typeof(EnumsLocalization))]
        Initiator = 1,

        [LocalizedDescription("Procedural", typeof(EnumsLocalization))]
        Procedural = 2,

        [LocalizedDescription("Terminator", typeof(EnumsLocalization))]
        Terminator = 3
    }

    /// <summary>
    /// The workflow step category class.
    /// </summary>
    public class WorkflowStepCategory : BaseEntity<int>
    {
        /// <summary>
        /// The workflow category Arabic name.
        /// </summary>
        public string CategoryArName { get; set; }

        /// <summary>
        /// The workflow steps  navigatino property.
        /// </summary>
        public virtual ICollection<WorkflowStep> WorkflowSteps { get; set; } = new List<WorkflowStep>();
    }
}
