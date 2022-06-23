using Moe.La.Core.Dtos.Workflow;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.Core.Interfaces.Services
{
    public interface IWorkflowLookupsService
    {
        #region WorkflowStatuses

        /// <summary>
        /// Get all workflow statuses.
        /// </summary>
        /// <returns></returns>
        Task<ReturnResult<IList<WorkflowStatusDto>>> GetWorkflowStatusesAsync();
        #endregion

        #region WorkflowTypes

        /// <summary>
        /// Get all workflow types.
        /// </summary>
        /// <returns></returns>
        Task<ReturnResult<IList<WorkflowTypeDto>>> GetWorkflowTypesAsync();

        /// <summary>
        /// Get the full workflow type data view.
        /// </summary>
        /// <param name="id">Workflow type ID.</param>
        /// <returns></returns>
        Task<ReturnResult<WorkflowTypeViewDto>> GetWorkflowTypeAsync(Guid id);
        #endregion

        #region WorkflowSteps

        /// <summary>
        /// Get workflow step.
        /// </summary>
        /// <param name="id">Workflow step ID.</param>
        /// <returns></returns>
        Task<ReturnResult<WorkflowStepDto>> GetWorkflowStepAsync(Guid id);


        /// <summary>
        /// Get all workflow steps.
        /// </summary>
        /// <returns></returns>
        Task<ReturnResult<IList<WorkflowStepListItemDto>>> GetWorkflowStepsAsync();

        /// <summary>
        /// Get all workflow steps for a given workflow type.
        /// </summary>
        /// <param name="workflowTypeId">Workflow type ID.</param>
        /// <returns></returns>
        Task<ReturnResult<IList<WorkflowStepListItemDto>>> GetWorkflowStepsAsync(Guid workflowTypeId);

        #endregion

        #region WorkflowActions

        /// <summary>
        /// Get workflow action.
        /// </summary>
        /// <param name="id">Workflow action ID.</param>
        /// <returns></returns>
        Task<ReturnResult<WorkflowActionDto>> GetWorkflowActionAsync(Guid id);

        /// <summary>
        /// Get all workflow actions.
        /// </summary>
        /// <returns></returns>
        Task<ReturnResult<IList<WorkflowActionListItemDto>>> GetWorkflowActionsAsync();

        /// <summary>
        /// Get all workflow actions.
        /// </summary>
        /// <param name="workflowTypeId">Workflow type ID.</param>
        /// <returns></returns>
        Task<ReturnResult<IList<WorkflowActionListItemDto>>> GetWorkflowActionsAsync(Guid workflowTypeId);
        #endregion

        #region WorkflowStepActions

        /// <summary>
        /// Get workflow step action.
        /// </summary>
        /// <param name="id">workflow step action ID.</param>
        /// <returns></returns>
        Task<ReturnResult<WorkflowStepActionDto>> GetWorkflowStepActionAsync(Guid id);

        /// <summary>
        /// Get all workflow step actions.
        /// </summary>
        /// <param name="workflowStepId">Workflow step ID.</param>
        /// <returns></returns>
        Task<ReturnResult<IList<WorkflowInstanceActionDto>>> GetWorkflowStepActionsAsync(Guid workflowStepId);

        /// <summary>
        /// Get all workflow step actions for a given workflow type.
        /// </summary>
        /// <param name="workflowTypeId">Workflow type ID.</param>
        /// <returns></returns>
        Task<ReturnResult<IList<WorkflowStepActionListItemDto>>> GetWorkflowStepActionsByWorkflowTypeAsync(Guid workflowTypeId);
        #endregion
    }
}
