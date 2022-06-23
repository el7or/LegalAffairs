using Moe.La.Core.Dtos.Workflow;
using Moe.La.Core.Models;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IWorkflowConfigurationService
    {
        Task<ReturnResult<WorkflowTypeDto>> AddWorkflowTypeAsync(WorkflowTypeDto workflowType);

        Task<ReturnResult<WorkflowTypeDto>> EditWorkflowTypeAsync(WorkflowTypeDto workflowType);

        Task<ReturnResult<WorkflowStepDto>> AddWorkflowStepAsync(WorkflowStepDto workflowStep);

        Task<ReturnResult<WorkflowStepDto>> EditWorkflowStepAsync(WorkflowStepDto workflowStep);

        Task<ReturnResult<WorkflowActionDto>> AddWorkflowActionAsync(WorkflowActionDto workflowAction);

        Task<ReturnResult<WorkflowActionDto>> EditWorkflowActionAsync(WorkflowActionDto workflowAction);

        Task<ReturnResult<WorkflowStepActionDto>> AddWorkflowStepActionAsync(WorkflowStepActionDto workflowStepAction);

        Task<ReturnResult<WorkflowStepActionDto>> UpdateWorkflowStepActionAsync(WorkflowStepActionDto workflowStepAction);

        Task<ReturnResult<WorkflowStepDto>> GetWorkflowInitiatorAsync(Guid workflowTypeId);

        Task<ReturnResult<WorkflowStepActionDto>> GetWorkflowStepActionAsync(Guid workflowStepId, Guid workflowActionId);

        Task<ReturnResult<WorkflowStepPermissionDto>> AddWorkflowStepPermissionAsync(WorkflowStepPermissionDto workflowStepPermission);
    }
}
