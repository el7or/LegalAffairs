using Moe.La.Core.Dtos.Workflow;

namespace Moe.La.UnitTests.Builders
{
    public class WorkflowStatusBuilder
    {
        private WorkflowStatusDto _workflowStatus = new WorkflowStatusDto();

        public WorkflowStatusBuilder StatusArName(string statusArName)
        {
            _workflowStatus.StatusArName = statusArName;
            return this;
        }

        public WorkflowStatusBuilder WithDefaultValues()
        {
            _workflowStatus = new WorkflowStatusDto
            {
                StatusArName = "جديد"
            };

            return this;
        }

        public WorkflowStatusDto Build() => _workflowStatus;
    }
}
