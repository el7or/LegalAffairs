using Moe.La.Core.Dtos.Workflow;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IWorkflowInstanceService
    {
        Task<ReturnResult<WorkflowInstanceDto>> AddWorkflowInstanceAsync(WorkflowInstanceDto workflowInstance, Guid workflowActionId, string customData = null);

        Task<ReturnResult<bool>> ProcessWorkflowInstanceAsync(Guid workflowInstanceId, Guid workflowActionId, string processNote, Guid? assignedTo = null);

        Task<ReturnResult<WorkflowInstanceLogDto>> AddWorkflowInstanceLogAsync(WorkflowInstanceDto workflowInstance, Guid workflowActionId, string note = null);

        Task<ReturnResult<WorkflowInstanceLogDto>> AddWorkflowInstanceLogAsync(Guid workflowInstanceId, Guid workflowStepId, int workflowStatusId, Guid workflowActionId, string note = null);

        Task<ReturnResult<WorkflowInstanceDto>> GetWorkflowInstanceAsync(Guid workflowInstanceId);

        Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(Guid workflowTypeId, Guid? workflowStepId, int? workflowStatusId);

        Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(List<int> permissions, Guid? lockedBy, Guid? claimedBy);

        Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(Guid createdFor, Guid? workflowTypeId, IList<int> statuses);

        Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(Guid lockedBy, IList<int> statuses);

        Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(Guid assignedTo);

        Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(Guid[] workflowInstancesIds);

        Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(Guid? lockedBy, Guid? claimedBy);

        Task<ReturnResult<bool>> UpdateWorkflowInstanceAsync(Guid workflowInstanceId, Guid? lockedBy, Guid? assignedTo, Guid? claimedBy);

        Task<ReturnResult<WorkflowInstanceActionDto>> GetWorkflowInstanceActionAsync(Guid workflowStepId, Guid workflowActionId);

        Task<ReturnResult<bool>> SetInstanceLockAsync(Guid workflowInstanceId, bool isLocked, Guid? lockedBy);

        Task<ReturnResult<bool>> UnlockAllWorkflowInstancesAsync(Guid workflowTypeId);

        Task<ReturnResult<bool>> UnlockAllWorkflowInstancesAsync(Guid lockedBy, bool all = true);

        Task<ReturnResult<IList<WorkflowInstanceActionDto>>> GetWorkflowInstanceAvailableActionsAsync(Guid workflowInstanceId);

        Task<ReturnResult<bool>> RollBackWorkflowInstanceAsync(Guid workflowInstanceId);
    }
}
