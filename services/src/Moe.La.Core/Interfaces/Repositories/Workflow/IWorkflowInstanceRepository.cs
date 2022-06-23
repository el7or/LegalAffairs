using Moe.La.Core.Dtos.Workflow;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IWorkflowInstanceRepository
    {
        Task AddWorkflowInstanceAsync(WorkflowInstanceDto workflowInstance);

        Task AddWorkflowInstanceLogAsync(WorkflowInstanceLogDto workflowInstanceLog);

        Task<WorkflowInstanceDto> GetWorkflowInstanceAsync(Guid workflowInstanceId);

        Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(Guid workflowTypeId, Guid? workflowStepId, int? workflowStatusId);

        Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(List<int> permissions, Guid? lockedBy, Guid? claimedBy);

        Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(Guid createdFor, Guid? workflowTypeId, IList<int> statuses);

        Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(Guid lockedBy, IList<int> statuses);

        Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(Guid assignedTo);

        Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(Guid[] workflowInstancesIds);

        Task<IList<WorkflowInstanceDto>> GetWorkflowInstancesAsync(Guid? lockedBy, Guid? claimedBy);

        Task UpdateWorkflowInstanceAsync(WorkflowInstanceDto workflowInstance);

        Task<WorkflowInstanceActionDto> GetWorkflowInstanceActionAsync(Guid workflowStepId, Guid workflowActionId);

        Task SetInstanceLockAsync(WorkflowInstanceDto workflowInstance);

        Task UnlockAllWorkflowInstancesAsync(Guid workflowTypeId);

        Task UnlockAllWorkflowInstancesAsync(Guid lockedBy, bool all = true);

        Task<IList<WorkflowInstanceActionDto>> GetWorkflowInstanceAvailableActionsAsync(Guid workflowInstanceId);

        Task RollBackWorkflowInstanceAsync(WorkflowInstanceDto workflowInstance);

        Task DeleteWorkflowInstanceLogAsync(Guid workflowInstanceLogId);

        Task<IList<WorkflowInstanceLogDto>> GetWorkflowInstanceLogsAsync(Guid workflowInstanceId);
    }
}
