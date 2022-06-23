using Microsoft.Extensions.Logging;
using Moe.La.Common.Resources;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos.Workflow;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using Moe.La.ServiceInterface.Validators.Workflow;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class WorkflowConfigurationService : IWorkflowConfigurationService
    {
        private readonly IWorkflowConfigurationRepository _workflowConfigurationRepository;
        private readonly ILogger<WorkflowConfigurationService> _logger;
        private readonly Localization<Messages> _localization;

        public WorkflowConfigurationService(IWorkflowConfigurationRepository workflowConfigurationRepository,
            ILogger<WorkflowConfigurationService> logger,
            Localization<Messages> localization = null)
        {
            _workflowConfigurationRepository = workflowConfigurationRepository;
            _logger = logger;
            _localization = localization;
        }


        #region WorkflowStatuses

        #endregion

        #region WorkflowTypes
        public async Task<ReturnResult<WorkflowTypeDto>> AddWorkflowTypeAsync(WorkflowTypeDto workflowType)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new WorkflowTypeValidator(), workflowType);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<WorkflowTypeDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _workflowConfigurationRepository.AddWorkflowTypeAsync(workflowType);

                return new ReturnResult<WorkflowTypeDto>(true, HttpStatuses.Status201Created, workflowType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowType);

                return new ReturnResult<WorkflowTypeDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { _localization?.Translate(Messages.InternalServerError) }
                };
            }
        }

        public async Task<ReturnResult<WorkflowTypeDto>> EditWorkflowTypeAsync(WorkflowTypeDto workflowType)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new WorkflowTypeValidator(), workflowType);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<WorkflowTypeDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _workflowConfigurationRepository.EditWorkflowTypeAsync(workflowType);

                return new ReturnResult<WorkflowTypeDto>(true, HttpStatuses.Status200OK, workflowType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowType);

                return new ReturnResult<WorkflowTypeDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { _localization?.Translate(Messages.InternalServerError) }
                };
            }
        }
        #endregion

        #region WorkflowSteps
        public async Task<ReturnResult<WorkflowStepDto>> AddWorkflowStepAsync(WorkflowStepDto workflowStep)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new WorkflowStepValidator(), workflowStep);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<WorkflowStepDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _workflowConfigurationRepository.AddWorkflowStepAsync(workflowStep);

                return new ReturnResult<WorkflowStepDto>(true, HttpStatuses.Status201Created, workflowStep);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowStep);

                return new ReturnResult<WorkflowStepDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { _localization?.Translate(Messages.InternalServerError) }
                };
            }
        }

        public async Task<ReturnResult<WorkflowStepDto>> EditWorkflowStepAsync(WorkflowStepDto workflowStep)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new WorkflowStepValidator(), workflowStep);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<WorkflowStepDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _workflowConfigurationRepository.EditWorkflowStepAsync(workflowStep);

                return new ReturnResult<WorkflowStepDto>(true, HttpStatuses.Status200OK, workflowStep);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowStep);

                return new ReturnResult<WorkflowStepDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { _localization?.Translate(Messages.InternalServerError) }
                };
            }
        }
        #endregion

        #region WorkflowActions
        public async Task<ReturnResult<WorkflowActionDto>> AddWorkflowActionAsync(WorkflowActionDto workflowAction)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new WorkflowActionValidator(), workflowAction);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<WorkflowActionDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _workflowConfigurationRepository.AddWorkflowActionAsync(workflowAction);

                return new ReturnResult<WorkflowActionDto>(true, HttpStatuses.Status201Created, workflowAction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowAction);

                return new ReturnResult<WorkflowActionDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { _localization?.Translate(Messages.InternalServerError) }
                };
            }
        }

        public async Task<ReturnResult<WorkflowActionDto>> EditWorkflowActionAsync(WorkflowActionDto workflowAction)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new WorkflowActionValidator(), workflowAction);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<WorkflowActionDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _workflowConfigurationRepository.EditWorkflowActionAsync(workflowAction);

                return new ReturnResult<WorkflowActionDto>(true, HttpStatuses.Status200OK, workflowAction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowAction);

                return new ReturnResult<WorkflowActionDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { _localization?.Translate(Messages.InternalServerError) }
                };
            }
        }
        #endregion

        #region WorkflowStepsCategories

        #endregion

        #region WorkflowStepActions

        public async Task<ReturnResult<WorkflowStepActionDto>> GetWorkflowStepActionAsync(Guid workflowStepId, Guid workflowActionId)
        {
            try
            {
                var workflowStepAction = await _workflowConfigurationRepository.GetWorkflowStepActionAsync(workflowStepId, workflowActionId);

                if (workflowStepAction is null)
                {
                    return new ReturnResult<WorkflowStepActionDto>(false, HttpStatuses.Status404NotFound, new List<string> { _localization?.Translate(Messages.NotFound) });
                }

                return new ReturnResult<WorkflowStepActionDto>(true, HttpStatuses.Status200OK, workflowStepAction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowStepId, workflowActionId);

                return new ReturnResult<WorkflowStepActionDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { _localization?.Translate(Messages.InternalServerError) }
                };
            }
        }

        public async Task<ReturnResult<WorkflowStepActionDto>> AddWorkflowStepActionAsync(WorkflowStepActionDto workflowStepAction)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new WorkflowStepActionValidator(), workflowStepAction);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<WorkflowStepActionDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _workflowConfigurationRepository.AddWorkflowStepActionAsync(workflowStepAction);

                return new ReturnResult<WorkflowStepActionDto>(true, HttpStatuses.Status201Created, workflowStepAction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowStepAction);

                return new ReturnResult<WorkflowStepActionDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { _localization?.Translate(Messages.InternalServerError) }
                };
            }
        }

        public async Task<ReturnResult<WorkflowStepActionDto>> UpdateWorkflowStepActionAsync(WorkflowStepActionDto workflowStepAction)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new WorkflowStepActionValidator(), workflowStepAction);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<WorkflowStepActionDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _workflowConfigurationRepository.EditWorkflowStepActionAsync(workflowStepAction);

                return new ReturnResult<WorkflowStepActionDto>(true, HttpStatuses.Status200OK, workflowStepAction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowStepAction);

                return new ReturnResult<WorkflowStepActionDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        #endregion

        #region WorkflowStepPermission
        public async Task<ReturnResult<WorkflowStepPermissionDto>> AddWorkflowStepPermissionAsync(WorkflowStepPermissionDto workflowStepPermission)
        {
            try
            {
                var validationResult = ValidationResult.CheckModelValidation(new WorkflowStepPermissionValidator(), workflowStepPermission);

                if (!validationResult.IsValid)
                {
                    return new ReturnResult<WorkflowStepPermissionDto>(false, HttpStatuses.Status400BadRequest, validationResult.Errors);
                }

                await _workflowConfigurationRepository.AddWorkflowStepPermissionAsync(workflowStepPermission);

                return new ReturnResult<WorkflowStepPermissionDto>(true, HttpStatuses.Status201Created, workflowStepPermission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowStepPermission);

                return new ReturnResult<WorkflowStepPermissionDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { _localization?.Translate(Messages.InternalServerError) }
                };
            }
        }

        #endregion

        public async Task<ReturnResult<WorkflowStepDto>> GetWorkflowInitiatorAsync(Guid workflowTypeId)
        {
            try
            {
                var workflowStep = await _workflowConfigurationRepository.GetWorkflowInitiatorAsync(workflowTypeId);

                if (workflowStep is null)
                {
                    return new ReturnResult<WorkflowStepDto>(false, HttpStatuses.Status404NotFound, new List<string> { _localization?.Translate(Messages.NotFound) });
                }

                return new ReturnResult<WorkflowStepDto>(true, HttpStatuses.Status200OK, workflowStep);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowTypeId);

                return new ReturnResult<WorkflowStepDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatuses.Status500InternalServerError,
                    ErrorList = new List<string> { _localization?.Translate(Messages.InternalServerError) }
                };
            }
        }
    }
}
