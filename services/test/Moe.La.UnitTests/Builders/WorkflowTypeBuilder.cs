using Moe.La.Core.Dtos.Workflow;

namespace Moe.La.UnitTests.Builders
{
    public class WorkflowTypeBuilder
    {
        private WorkflowTypeDto _workflowType = new WorkflowTypeDto();

        public WorkflowTypeBuilder TypeArName(string typeArName)
        {
            _workflowType.TypeArName = typeArName;
            return this;
        }

        public WorkflowTypeBuilder IsActive(bool isActive)
        {
            _workflowType.IsActive = isActive;
            return this;
        }

        public WorkflowTypeBuilder LockPeriod(int lockPeriod)
        {
            _workflowType.LockPeriod = lockPeriod;
            return this;
        }

        public WorkflowTypeBuilder WithDefaultValues()
        {
            _workflowType = new WorkflowTypeDto
            {
                TypeArName = "إنشاء قضية جديدة",
                IsActive = true,
                LockPeriod = 48
            };

            return this;
        }

        public WorkflowTypeDto Build() => _workflowType;
    }
}
