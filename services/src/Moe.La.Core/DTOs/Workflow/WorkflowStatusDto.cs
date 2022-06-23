namespace Moe.La.Core.Dtos.Workflow
{
    public class WorkflowStatusDto : BaseDto<int>
    {
        /// <summary>
        /// The workflow status Arabic name.
        /// </summary>
        public string StatusArName { get; set; }

    }
}
