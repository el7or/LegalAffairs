using Moe.La.Core.Dtos.Workflow;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Repositories
{
    public interface IWorkflowLookupsRepository
    {
        #region WorkflowStatuses

        /// <summary>
        /// Get all workflow statuses.
        /// </summary>
        /// <returns></returns>
        Task<IList<WorkflowStatusDto>> GetWorkflowStatusesAsync();
        #endregion

        #region WorkflowTypes

        /// <summary>
        /// Get all workflow types.
        /// </summary>
        /// <returns></returns>
        Task<IList<WorkflowTypeDto>> GetWorkflowTypesAsync();

        /// <summary>
        /// Get the full workflow type view data.
        /// </summary>
        /// <param name="id">Workflow type ID.</param>
        /// <returns></returns>
        Task<WorkflowTypeViewDto> GetWorkflowTypeAsync(Guid id);
        #endregion

        #region WorkflowSteps

        /// <summary>
        /// Get workflow step
        /// </summary>
        /// <param name="id">Workflow step ID.</param>
        /// <returns></returns>
        Task<WorkflowStepDto> GetWorkflowStepAsync(Guid id);

        /// <summary>
        /// Get all workflow steps.
        /// </summary>
        /// <returns></returns>
        Task<IList<WorkflowStepListItemDto>> GetWorkflowStepsAsync();

        /// <summary>
        /// Get all workflow steps for a given workflow type.
        /// </summary>
        /// <param name="workflowTypeId">Workflow type ID.</param>
        /// <returns></returns>
        Task<IList<WorkflowStepListItemDto>> GetWorkflowStepsAsync(Guid workflowTypeId);
        #endregion

        #region WorkflowActions

        /// <summary>
        /// Get workflow action.
        /// </summary>
        /// <param name="id">Workflow action ID.</param>
        /// <returns></returns>
        Task<WorkflowActionDto> GetWorkflowActionAsync(Guid id);

        /// <summary>
        /// Get all workflow actions.
        /// </summary>
        /// <returns></returns>
        Task<IList<WorkflowActionListItemDto>> GetWorkflowActionsAsync();

        /// <summary>
        /// Get all workflow actions for a given workflow type.
        /// </summary>
        /// <param name="workflowTypeId">Workflow type ID></param>
        /// <returns></returns>
        Task<IList<WorkflowActionListItemDto>> GetWorkflowActionsAsync(Guid workflowTypeId);
        #endregion

        #region WorkflowStepsCategories

        /// <summary>
        /// Get all workflow action steps categories.
        /// </summary>
        /// <returns></returns>
        Task<IList<WorkflowStepCategoryDto>> GetWorkflowStepsCategoriesAsync();
        #endregion

        #region WorkflowStepActions

        /// <summary>
        /// Get workflow step action.
        /// </summary>
        /// <param name="id">Workflow step action ID.</param>
        /// <returns></returns>
        Task<WorkflowStepActionDto> GetWorkflowStepActionAsync(Guid id);

        /// <summary>
        /// Get all workflow step actions for a given workflow step.
        /// </summary>
        /// <param name="workflowStepId">Workflow step ID.</param>
        /// <returns></returns>
        Task<IList<WorkflowInstanceActionDto>> GetWorkflowStepActionsByWorkflowStepAsync(Guid workflowStepId);

        /// <summary>
        /// Get workflow step actions for a given workflow type.
        /// </summary>
        /// <param name="workflowTypeId">Workflow type ID.</param>
        /// <returns></returns>
        Task<IList<WorkflowStepActionListItemDto>> GetWorkflowStepActionsByWorkflowTypeAsync(Guid workflowTypeId);
        #endregion
    }
}
