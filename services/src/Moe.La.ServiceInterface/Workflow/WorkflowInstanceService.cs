using Microsoft.Extensions.Logging;
using Moe.La.Common.Resources;
using Moe.La.Common.Resources.Common;
using Moe.La.Core.Dtos.Workflow;
using Moe.La.Core.Entities;
using Moe.La.Core.Enums;
using Moe.La.Core.Interfaces.Repositories;
using Moe.La.Core.Interfaces.Services;
using Moe.La.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moe.La.ServiceInterface
{
    public class WorkflowInstanceService : IWorkflowInstanceService
    {
        private readonly IWorkflowInstanceRepository _workflowInstanceRepository;
        private readonly IWorkflowConfigurationService _workflowConfigurationService;
        private readonly ILogger<WorkflowInstanceService> _logger;
        private readonly Localization<Messages> _localization;

        public WorkflowInstanceService(IWorkflowInstanceRepository workflowInstanceRepository,
            IWorkflowConfigurationService workflowConfigurationService,
            ILogger<WorkflowInstanceService> logger,
            Localization<Messages> localization = null)
        {
            _workflowInstanceRepository = workflowInstanceRepository;
            _workflowConfigurationService = workflowConfigurationService;
            _logger = logger;
            _localization = localization;
        }

        public async Task<ReturnResult<WorkflowInstanceDto>> AddWorkflowInstanceAsync(WorkflowInstanceDto workflowInstance, Guid workflowActionId, string customData = null)
        {
            try
            {
                var workflowStepResult = await _workflowConfigurationService.GetWorkflowInitiatorAsync(workflowInstance.WorkflowTypeId);

                if (!workflowStepResult.IsSuccess)
                {
                    _logger.LogError($"Invalid configuration for the WorkflowInstanceId: {workflowInstance.Id}. No initiator step exists.", workflowInstance);

                    return new ReturnResult<WorkflowInstanceDto>(false, HttpStatuses.Status400BadRequest, new List<string> { $"لا توجد تهيئة لسير العمل رقم {workflowInstance.Id}." });
                }

                var workflowStepActionResult = await _workflowConfigurationService.GetWorkflowStepActionAsync(workflowStepResult.Data.Id, workflowActionId);

                if (!workflowStepActionResult.IsSuccess)
                {
                    _logger.LogError($"Invalid configuration for the WorkflowInstanceId: {workflowInstance.Id}. No step action combination exists: WorkflowStepId: {workflowStepResult.Data.Id}, WorkflowActionId: {workflowActionId}.");

                    return new ReturnResult<WorkflowInstanceDto>(false, HttpStatuses.Status400BadRequest, new List<string> { $"حدث خطأ أثناء تهيئة سير العمل رقم {workflowInstance.Id}." });
                };

                // Prepare the workflow instance.
                workflowInstance.CurrentStepId = workflowStepActionResult.Data.NextStepId;
                workflowInstance.CurrentStatusId = workflowStepActionResult.Data.NextStatusId;

                await _workflowInstanceRepository.AddWorkflowInstanceAsync(workflowInstance);
                await AddWorkflowInstanceLogAsync(workflowInstance.Id, workflowStepResult.Data.Id, (int)WorkflowStatuses.New, workflowActionId, customData);

                return new ReturnResult<WorkflowInstanceDto>(true, HttpStatuses.Status201Created, workflowInstance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowInstance, workflowActionId);

                return new ReturnResult<WorkflowInstanceDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        public async Task<ReturnResult<WorkflowInstanceLogDto>> AddWorkflowInstanceLogAsync(WorkflowInstanceDto workflowInstance, Guid workflowActionId, string note = null)
        {
            try
            {
                var workflowInstanceLog = new WorkflowInstanceLogDto
                {
                    Id = Guid.NewGuid(),
                    WorkflowInstanceId = workflowInstance.Id,
                    WorkflowStepId = workflowInstance.CurrentStepId,
                    WorkflowStatusId = workflowInstance.CurrentStatusId,
                    WorkflowActionId = workflowActionId,
                    WorkflowInstanceNote = note
                };

                await _workflowInstanceRepository.AddWorkflowInstanceLogAsync(workflowInstanceLog);

                return new ReturnResult<WorkflowInstanceLogDto>(true, HttpStatuses.Status201Created, workflowInstanceLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowInstance, workflowActionId);

                return new ReturnResult<WorkflowInstanceLogDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="workflowStepId"></param>
        /// <param name="workflowStatusId"></param>
        /// <param name="workflowActionId"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public async Task<ReturnResult<WorkflowInstanceLogDto>> AddWorkflowInstanceLogAsync(Guid workflowInstanceId, Guid workflowStepId, int workflowStatusId, Guid workflowActionId, string note = null)
        {
            try
            {
                var workflowInstanceLog = new WorkflowInstanceLogDto
                {
                    Id = Guid.NewGuid(),
                    WorkflowInstanceId = workflowInstanceId,
                    WorkflowStepId = workflowStepId,
                    WorkflowStatusId = workflowStatusId,
                    WorkflowActionId = workflowActionId,
                    WorkflowInstanceNote = note
                };

                await _workflowInstanceRepository.AddWorkflowInstanceLogAsync(workflowInstanceLog);

                return new ReturnResult<WorkflowInstanceLogDto>(true, HttpStatuses.Status201Created, workflowInstanceLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowInstanceId, workflowStepId, workflowStatusId, workflowActionId);

                return new ReturnResult<WorkflowInstanceLogDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowStepId"></param>
        /// <param name="workflowActionId"></param>
        /// <returns></returns>
        public async Task<ReturnResult<WorkflowInstanceActionDto>> GetWorkflowInstanceActionAsync(Guid workflowStepId, Guid workflowActionId)
        {
            try
            {
                var workflowInstanceAction = await _workflowInstanceRepository.GetWorkflowInstanceActionAsync(workflowStepId, workflowActionId);

                if (workflowInstanceAction is null)
                {
                    return new ReturnResult<WorkflowInstanceActionDto>(false, HttpStatuses.Status404NotFound, new List<string> { _localization?.Translate(Messages.NotFound) });
                }

                return new ReturnResult<WorkflowInstanceActionDto>(true, HttpStatuses.Status200OK, workflowInstanceAction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowStepId, workflowActionId);

                return new ReturnResult<WorkflowInstanceActionDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public async Task<ReturnResult<WorkflowInstanceDto>> GetWorkflowInstanceAsync(Guid workflowInstanceId)
        {
            try
            {
                var workflowInstance = await _workflowInstanceRepository.GetWorkflowInstanceAsync(workflowInstanceId);

                if (workflowInstance is null)
                {
                    return new ReturnResult<WorkflowInstanceDto>(false, HttpStatuses.Status404NotFound, new List<string> { _localization?.Translate(Messages.NotFound) });
                }

                return new ReturnResult<WorkflowInstanceDto>(true, HttpStatuses.Status200OK, workflowInstance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowInstanceId);

                return new ReturnResult<WorkflowInstanceDto>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public async Task<ReturnResult<IList<WorkflowInstanceActionDto>>> GetWorkflowInstanceAvailableActionsAsync(Guid workflowInstanceId)
        {
            try
            {
                var workflowInstanceActions = await _workflowInstanceRepository.GetWorkflowInstanceAvailableActionsAsync(workflowInstanceId);

                if (workflowInstanceActions is null)
                {
                    return new ReturnResult<IList<WorkflowInstanceActionDto>>(false, HttpStatuses.Status404NotFound, new List<string> { _localization?.Translate(Messages.NotFound) });
                }

                return new ReturnResult<IList<WorkflowInstanceActionDto>>(true, HttpStatuses.Status200OK, workflowInstanceActions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowInstanceId);

                return new ReturnResult<IList<WorkflowInstanceActionDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowTypeId"></param>
        /// <param name="workflowStepId"></param>
        /// <param name="workflowStatusId"></param>
        /// <returns></returns>
        public async Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(Guid workflowTypeId, Guid? workflowStepId, int? workflowStatusId)
        {
            try
            {
                var workflowInstances = await _workflowInstanceRepository.GetWorkflowInstancesAsync(workflowTypeId, workflowStepId, workflowStatusId);

                if (workflowInstances is null)
                {
                    return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status404NotFound, new List<string> { _localization?.Translate(Messages.NotFound) });
                }

                return new ReturnResult<IList<WorkflowInstanceDto>>(true, HttpStatuses.Status200OK, workflowInstances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowTypeId, workflowStepId, workflowStatusId);

                return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="lockedBy"></param>
        /// <param name="claimedBy"></param>
        /// <returns></returns>
        public async Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(List<int> permissions, Guid? lockedBy, Guid? claimedBy)
        {
            try
            {
                var workflowInstances = await _workflowInstanceRepository.GetWorkflowInstancesAsync(permissions, lockedBy, claimedBy);

                if (workflowInstances is null)
                {
                    return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status404NotFound, new List<string> { _localization?.Translate(Messages.NotFound) });
                }

                return new ReturnResult<IList<WorkflowInstanceDto>>(true, HttpStatuses.Status200OK, workflowInstances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, permissions, lockedBy, claimedBy);

                return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createdFor"></param>
        /// <param name="workflowTypeId"></param>
        /// <param name="statuses"></param>
        /// <returns></returns>
        public async Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(Guid createdFor, Guid? workflowTypeId, IList<int> statuses)
        {
            try
            {
                var workflowInstances = await _workflowInstanceRepository.GetWorkflowInstancesAsync(createdFor, workflowTypeId, statuses);

                if (workflowInstances is null)
                {
                    return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status404NotFound, new List<string> { _localization?.Translate(Messages.NotFound) });
                }

                return new ReturnResult<IList<WorkflowInstanceDto>>(true, HttpStatuses.Status200OK, workflowInstances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, createdFor, workflowTypeId, statuses);

                return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lockedBy"></param>
        /// <param name="statuses"></param>
        /// <returns></returns>
        public async Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(Guid lockedBy, IList<int> statuses)
        {
            try
            {
                var workflowInstances = await _workflowInstanceRepository.GetWorkflowInstancesAsync(lockedBy, statuses);

                if (workflowInstances is null)
                {
                    return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status404NotFound, new List<string> { _localization?.Translate(Messages.NotFound) });
                }

                return new ReturnResult<IList<WorkflowInstanceDto>>(true, HttpStatuses.Status200OK, workflowInstances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, lockedBy, statuses);

                return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assignedTo"></param>
        /// <returns></returns>
        public async Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(Guid assignedTo)
        {
            try
            {
                var workflowInstances = await _workflowInstanceRepository.GetWorkflowInstancesAsync(assignedTo);

                if (workflowInstances is null)
                {
                    return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status404NotFound, new List<string> { _localization.Translate(Messages.NotFound) });
                }

                return new ReturnResult<IList<WorkflowInstanceDto>>(true, HttpStatuses.Status200OK, workflowInstances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, assignedTo);

                return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowInstancesIds"></param>
        /// <returns></returns>
        public async Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(Guid[] workflowInstancesIds)
        {
            try
            {
                var workflowInstances = await _workflowInstanceRepository.GetWorkflowInstancesAsync(workflowInstancesIds);

                if (workflowInstances is null)
                {
                    return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status404NotFound, new List<string> { _localization.Translate(Messages.NotFound) });
                }

                return new ReturnResult<IList<WorkflowInstanceDto>>(true, HttpStatuses.Status200OK, workflowInstances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowInstancesIds);

                return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lockedBy"></param>
        /// <param name="claimedBy"></param>
        /// <returns></returns>
        public async Task<ReturnResult<IList<WorkflowInstanceDto>>> GetWorkflowInstancesAsync(Guid? lockedBy, Guid? claimedBy)
        {
            try
            {
                var workflowInstances = await _workflowInstanceRepository.GetWorkflowInstancesAsync(lockedBy, claimedBy);

                if (workflowInstances is null)
                {
                    return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status404NotFound, new List<string> { _localization.Translate(Messages.NotFound) });
                }

                return new ReturnResult<IList<WorkflowInstanceDto>>(true, HttpStatuses.Status200OK, workflowInstances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, lockedBy, claimedBy);
                return new ReturnResult<IList<WorkflowInstanceDto>>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="workflowActionId"></param>
        /// <param name="processNote"></param>
        /// <param name="assignedTo"></param>
        /// <returns></returns>
        public async Task<ReturnResult<bool>> ProcessWorkflowInstanceAsync(Guid workflowInstanceId, Guid workflowActionId, string processNote, Guid? assignedTo = null)
        {
            try
            {
                var workflowInstance = await _workflowInstanceRepository.GetWorkflowInstanceAsync(workflowInstanceId);

                if (workflowInstance is null)
                {
                    _logger.LogError("Workflow instance doesn't exist.", workflowInstanceId);
                    return new ReturnResult<bool>(false, HttpStatuses.Status404NotFound, new List<string> { _localization.Translate(Messages.NotFound) });
                }

                var workflowStepActionResult = await _workflowConfigurationService.GetWorkflowStepActionAsync(workflowInstance.CurrentStepId, workflowActionId);

                if (!workflowStepActionResult.IsSuccess)
                {
                    _logger.LogError($"Invalid configuration with workflow instance #{workflowInstanceId}, no step action configuration exists. Workflow Step #{workflowInstance.CurrentStepId} Workflow Action #{workflowActionId}");
                }

                var oldWorkflowStepId = workflowInstance.CurrentStepId;
                var oldWorkflowStatusId = workflowInstance.CurrentStatusId;

                workflowInstance.CurrentStepId = workflowStepActionResult.Data.NextStepId;
                workflowInstance.CurrentStatusId = workflowStepActionResult.Data.NextStatusId;
                workflowInstance.LockedBy = null;
                workflowInstance.LockedOn = null;
                workflowInstance.ClaimedBy = null;
                workflowInstance.ClaimedOn = null;
                workflowInstance.AssignedTo = assignedTo;

                await _workflowInstanceRepository.UpdateWorkflowInstanceAsync(workflowInstance);
                await AddWorkflowInstanceLogAsync(workflowInstance.Id, oldWorkflowStepId, oldWorkflowStatusId, workflowActionId, processNote);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowInstanceId, workflowActionId, assignedTo);
                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public async Task<ReturnResult<bool>> RollBackWorkflowInstanceAsync(Guid workflowInstanceId)
        {
            try
            {
                var workflowInstance = await _workflowInstanceRepository.GetWorkflowInstanceAsync(workflowInstanceId);

                if (workflowInstance is null)
                {
                    return new ReturnResult<bool>(false, HttpStatuses.Status400BadRequest, new List<string> { "بيانات الطلب غير صالحة." });
                }

                if (workflowInstance.CurrentStatusId == (int)WorkflowStatuses.Uncompleted)
                {
                    await _workflowInstanceRepository.RollBackWorkflowInstanceAsync(workflowInstance);
                }
                else
                {
                    var lastWorkflowInstanceLog = (await _workflowInstanceRepository.GetWorkflowInstanceLogsAsync(workflowInstanceId)).FirstOrDefault();

                    if (lastWorkflowInstanceLog is null)
                    {
                        return new ReturnResult<bool>(false, HttpStatuses.Status400BadRequest, new List<string> { $"لا يوجد سجل تاريخي لسير العمل رقم {workflowInstanceId}." });
                    }

                    workflowInstance.CurrentStepId = lastWorkflowInstanceLog.WorkflowStepId;
                    workflowInstance.CurrentStatusId = lastWorkflowInstanceLog.WorkflowStatusId;
                    workflowInstance.LockedBy = null;
                    workflowInstance.LockedOn = null;
                    workflowInstance.ClaimedBy = null;
                    workflowInstance.ClaimedOn = null;
                    await _workflowInstanceRepository.UpdateWorkflowInstanceAsync(workflowInstance);
                    await _workflowInstanceRepository.DeleteWorkflowInstanceLogAsync(lastWorkflowInstanceLog.Id);
                }

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowInstanceId);
                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="isLocked"></param>
        /// <param name="lockedBy"></param>
        /// <returns></returns>
        public async Task<ReturnResult<bool>> SetInstanceLockAsync(Guid workflowInstanceId, bool isLocked, Guid? lockedBy)
        {
            try
            {
                var workflowInstance = await _workflowInstanceRepository.GetWorkflowInstanceAsync(workflowInstanceId);

                if (workflowInstance is null)
                {
                    return new ReturnResult<bool>(false, HttpStatuses.Status404NotFound, new List<string> { _localization.Translate(Messages.NotFound) });
                }

                if (workflowInstance.LockedOn.HasValue && isLocked)
                {
                    if (workflowInstance.LockedBy != lockedBy)
                    {
                        _logger.LogError($"Invalid operation: Trying to lock a workflow instance '{workflowInstanceId}' that is already locked by someone else.");
                        return new ReturnResult<bool>(false, HttpStatuses.Status400BadRequest, new List<string> { $"سير العمل رقم '{workflowInstanceId}' محجوز لدى مستخدم آخر." });
                    }
                }
                else if (workflowInstance.LockedOn.HasValue && !isLocked)
                {
                    workflowInstance.LockedBy = null;
                    workflowInstance.LockedOn = null;
                    await _workflowInstanceRepository.SetInstanceLockAsync(workflowInstance);
                }
                else if (!workflowInstance.LockedOn.HasValue && isLocked)
                {
                    if (!workflowInstance.ClaimedBy.HasValue)
                    {
                        workflowInstance.LockedOn = DateTime.Now;
                        workflowInstance.LockedBy = lockedBy;
                        await _workflowInstanceRepository.SetInstanceLockAsync(workflowInstance);
                    }
                }

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowInstanceId, isLocked, lockedBy);
                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowTypeId"></param>
        /// <returns></returns>
        public async Task<ReturnResult<bool>> UnlockAllWorkflowInstancesAsync(Guid workflowTypeId)
        {
            try
            {
                await _workflowInstanceRepository.UnlockAllWorkflowInstancesAsync(workflowTypeId);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowTypeId);
                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lockedBy"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        public async Task<ReturnResult<bool>> UnlockAllWorkflowInstancesAsync(Guid lockedBy, bool all = true)
        {
            try
            {
                await _workflowInstanceRepository.UnlockAllWorkflowInstancesAsync(lockedBy);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, lockedBy);
                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }


        public async Task<ReturnResult<bool>> UpdateWorkflowInstanceAsync(Guid workflowInstanceId, Guid? lockedBy, Guid? assignedTo, Guid? claimedBy)
        {
            try
            {
                var workflowInstance = await _workflowInstanceRepository.GetWorkflowInstanceAsync(workflowInstanceId);

                if (workflowInstance is null)
                {
                    _logger.LogError($"Workflow instance id '{workflowInstanceId}' doesn't exist.", workflowInstanceId);
                    return new ReturnResult<bool>(false, HttpStatuses.Status404NotFound, new List<string> { _localization.Translate(Messages.NotFound) });
                }

                if (lockedBy.HasValue)
                {
                    workflowInstance.LockedBy = lockedBy;
                    workflowInstance.LockedOn = DateTime.Now;
                }

                if (assignedTo.HasValue)
                {
                    workflowInstance.AssignedTo = assignedTo;
                }

                if (claimedBy.HasValue)
                {
                    workflowInstance.ClaimedBy = claimedBy;
                    workflowInstance.ClaimedOn = DateTime.Now;

                    // Release the lock.
                    workflowInstance.LockedBy = null;
                    workflowInstance.LockedOn = null;
                }

                await _workflowInstanceRepository.UpdateWorkflowInstanceAsync(workflowInstance);

                return new ReturnResult<bool>(true, HttpStatuses.Status200OK, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, workflowInstanceId, lockedBy, assignedTo, claimedBy);
                return new ReturnResult<bool>(false, HttpStatuses.Status500InternalServerError, new List<string> { _localization?.Translate(Messages.InternalServerError) });
            }
        }
    }
}
