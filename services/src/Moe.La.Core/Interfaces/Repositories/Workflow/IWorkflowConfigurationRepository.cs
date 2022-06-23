using Moe.La.Core.Dtos.Workflow;
using System;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    /// <summary>
    /// Workflow configuration repository.
    /// </summary>
    public interface IWorkflowConfigurationRepository
    {
        /// <summary>
        /// Add workflow type.
        /// </summary>
        /// <param name="workflowType"><see cref="WorkflowTypeDto"/> object.</param>
        /// <returns></returns>
        Task AddWorkflowTypeAsync(WorkflowTypeDto workflowType);

        /// <summary>
        /// Update workflow type.
        /// </summary>
        /// <param name="workflowType"><see cref="WorkflowTypeDto"/> object.</param>
        /// <returns></returns>
        Task EditWorkflowTypeAsync(WorkflowTypeDto workflowType);

        /// <summary>
        /// Add workflow step.
        /// </summary>
        /// <param name="workflowStep"><see cref="WorkflowStepDto"/> object.</param>
        /// <returns></returns>
        Task AddWorkflowStepAsync(WorkflowStepDto workflowStep);

        /// <summary>
        /// Update workflow step.
        /// </summary>
        /// <param name="workflowStep"><see cref="WorkflowStepDto"/> object.</param>
        /// <returns></returns>
        Task EditWorkflowStepAsync(WorkflowStepDto workflowStep);

        /// <summary>
        /// Add workflow action.
        /// </summary>
        /// <param name="workflowAction"><see cref="WorkflowActionDto"/> object.</param>
        /// <returns></returns>
        Task AddWorkflowActionAsync(WorkflowActionDto workflowAction);

        /// <summary>
        /// Edit workflow action.
        /// </summary>
        /// <param name="workflowAction"><see cref="WorkflowActionDto"/> object.</param>
        /// <returns></returns>
        Task EditWorkflowActionAsync(WorkflowActionDto workflowAction);

        /// <summary>
        /// Add workflow step action.
        /// </summary>
        /// <param name="workflowStepAction"><see cref="WorkflowStepActionDto"/> object.</param>
        /// <returns></returns>
        Task AddWorkflowStepActionAsync(WorkflowStepActionDto workflowStepAction);

        /// <summary>
        /// Edit workflow step action.
        /// </summary>
        /// <param name="workflowStepAction"><see cref="WorkflowStepActionDto"/> object.</param>
        /// <returns></returns>
        Task EditWorkflowStepActionAsync(WorkflowStepActionDto workflowStepAction);

        /// <summary>
        /// Add workflow step persmission.
        /// </summary>
        /// <param name="workflowStepPermission"><see cref="WorkflowStepPermissionDto"/> object.</param>
        /// <returns></returns>
        Task AddWorkflowStepPermissionAsync(WorkflowStepPermissionDto workflowStepPermission);

        /// <summary>
        /// Get the workflow initiator workflow step.
        /// </summary>
        /// <param name="workflowTypeId"><see cref="WorkflowType"/> id.</param>
        /// <returns></returns>
        Task<WorkflowStepDto> GetWorkflowInitiatorAsync(Guid workflowTypeId);

        /// <summary>
        /// Get the workflow step action.
        /// </summary>
        /// <param name="workflowStepId">Workflow step id.</param>
        /// <param name="workflowActionId">Workflow action id.</param>
        /// <returns></returns>
        Task<WorkflowStepActionDto> GetWorkflowStepActionAsync(Guid workflowStepId, Guid workflowActionId);
    }
}
