using Microsoft.Extensions.Logging;
using Moe.La.Common.Resources;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos.Workflow;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class WorkflowLookupsService : IWorkflowLookupsService
    {
        private readonly IWorkflowLookupsRepository _workflowLookupsRepository;
        private readonly ILogger<WorkflowLookupsService> _logger;
        private readonly Localization<Messages> _localization;

        public WorkflowLookupsService(IWorkflowLookupsRepository workflowLookupsRepository,
           ILogger<WorkflowLookupsService> logger,
           Localization<Messages> localization = null)
        {
            _workflowLookupsRepository = workflowLookupsRepository;
            _logger = logger;
            _localization = localization;
        }

        #region WorkflowAction

        public async Task<ReturnResult<WorkflowActionDto>> GetWorkflowActionAsync(Guid id)
        {
            try
            {
                var workflowActions = await _workflowLookupsRepository.GetWorkflowActionAsync(id);

                return new ReturnResult<WorkflowActionDto>(true, HttpStatuses.Status200OK, workflowActions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);
                return new ReturnResult<WorkflowActionDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization.Translate(Messages.InternalServerError) });
            }
        }

        public async Task<ReturnResult<IList<WorkflowActionListItemDto>>> GetWorkflowActionsAsync()
        {
            try
            {
                var workflowActions = await _workflowLookupsRepository.GetWorkflowActionsAsync();

                return new ReturnResult<IList<WorkflowActionListItemDto>>(true, HttpStatuses.Status200OK, workflowActions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ReturnResult<IList<WorkflowActionListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization.Translate(Messages.InternalServerError) });
            }
        }

        public async Task<ReturnResult<IList<WorkflowActionListItemDto>>> GetWorkflowActionsAsync(Guid workflowTypeId)
        {
            try
            {
                var workflowActions = await _workflowLookupsRepository.GetWorkflowActionsAsync(workflowTypeId);

                return new ReturnResult<IList<WorkflowActionListItemDto>>(true, HttpStatuses.Status200OK, workflowActions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowTypeId);
                return new ReturnResult<IList<WorkflowActionListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization.Translate(Messages.InternalServerError) });
            }
        }

        #endregion

        public async Task<ReturnResult<IList<WorkflowStatusDto>>> GetWorkflowStatusesAsync()
        {
            try
            {
                var workflowStatuses = await _workflowLookupsRepository.GetWorkflowStatusesAsync();

                return new ReturnResult<IList<WorkflowStatusDto>>(true, HttpStatuses.Status200OK, workflowStatuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ReturnResult<IList<WorkflowStatusDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization.Translate(Messages.InternalServerError) });
            }
        }

        public async Task<ReturnResult<WorkflowStepActionDto>> GetWorkflowStepActionAsync(Guid id)
        {
            try
            {
                var workflowStepAction = await _workflowLookupsRepository.GetWorkflowStepActionAsync(id);

                return new ReturnResult<WorkflowStepActionDto>(true, HttpStatuses.Status200OK, workflowStepAction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);
                return new ReturnResult<WorkflowStepActionDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization.Translate(Messages.InternalServerError) });
            }
        }

        public async Task<ReturnResult<IList<WorkflowInstanceActionDto>>> GetWorkflowStepActionsAsync(Guid workflowStepId)
        {
            try
            {
                var workflowStepActions = await _workflowLookupsRepository.GetWorkflowStepActionsByWorkflowStepAsync(workflowStepId);

                return new ReturnResult<IList<WorkflowInstanceActionDto>>(true, HttpStatuses.Status200OK, workflowStepActions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowStepId);
                return new ReturnResult<IList<WorkflowInstanceActionDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization.Translate(Messages.InternalServerError) });
            }
        }

        public async Task<ReturnResult<IList<WorkflowStepActionListItemDto>>> GetWorkflowStepActionsByWorkflowTypeAsync(Guid workflowTypeId)
        {
            try
            {
                var workflowStepActions = await _workflowLookupsRepository.GetWorkflowStepActionsByWorkflowTypeAsync(workflowTypeId);

                return new ReturnResult<IList<WorkflowStepActionListItemDto>>(true, HttpStatuses.Status200OK, workflowStepActions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowTypeId);
                return new ReturnResult<IList<WorkflowStepActionListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization.Translate(Messages.InternalServerError) });
            }
        }

        public async Task<ReturnResult<WorkflowStepDto>> GetWorkflowStepAsync(Guid id)
        {
            try
            {
                var workflowSteps = await _workflowLookupsRepository.GetWorkflowStepAsync(id);

                return new ReturnResult<WorkflowStepDto>(true, HttpStatuses.Status200OK, workflowSteps);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);
                return new ReturnResult<WorkflowStepDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization.Translate(Messages.InternalServerError) });
            }
        }

        public async Task<ReturnResult<IList<WorkflowStepListItemDto>>> GetWorkflowStepsAsync()
        {
            try
            {
                var workflowSteps = await _workflowLookupsRepository.GetWorkflowStepsAsync();

                return new ReturnResult<IList<WorkflowStepListItemDto>>(true, HttpStatuses.Status200OK, workflowSteps);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ReturnResult<IList<WorkflowStepListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization.Translate(Messages.InternalServerError) });
            }
        }

        public async Task<ReturnResult<IList<WorkflowStepListItemDto>>> GetWorkflowStepsAsync(Guid workflowTypeId)
        {
            try
            {
                var workflowSteps = await _workflowLookupsRepository.GetWorkflowStepsAsync(workflowTypeId);

                return new ReturnResult<IList<WorkflowStepListItemDto>>(true, HttpStatuses.Status200OK, workflowSteps);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowTypeId);
                return new ReturnResult<IList<WorkflowStepListItemDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization.Translate(Messages.InternalServerError) });
            }
        }

        public async Task<ReturnResult<IList<WorkflowTypeDto>>> GetWorkflowTypesAsync()
        {
            try
            {
                var workflowTypes = await _workflowLookupsRepository.GetWorkflowTypesAsync();

                return new ReturnResult<IList<WorkflowTypeDto>>(true, HttpStatuses.Status200OK, workflowTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ReturnResult<IList<WorkflowTypeDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization.Translate(Messages.InternalServerError) });
            }
        }


        public async Task<ReturnResult<WorkflowTypeViewDto>> GetWorkflowTypeAsync(Guid id)
        {
            try
            {
                var workflowTypes = await _workflowLookupsRepository.GetWorkflowTypeAsync(id);

                return new ReturnResult<WorkflowTypeViewDto>(true, HttpStatuses.Status200OK, workflowTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, id);
                return new ReturnResult<WorkflowTypeViewDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization.Translate(Messages.InternalServerError) });
            }
        }
    }
}
