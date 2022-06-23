using Microsoft.AspNetCore.Mvc;
using Moe.La.Common.Extensions;
using Moe.La.Core.Entities;
using Moe.La.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers.Workflow
{
    [Route("api/workflow-lookups")]
    [ApiController]
    public class WorkflowLookupsController : ControllerBase
    {
        private readonly IWorkflowLookupsService _workflowLookupsService;

        public WorkflowLookupsController(IWorkflowLookupsService workflowLookupsService)
        {
            _workflowLookupsService = workflowLookupsService;
        }

        #region WorkflowStatuses

        [HttpGet("workflow-statuses")]
        public async Task<IActionResult> GetWorkflowStatuses()
        {
            var result = await _workflowLookupsService.GetWorkflowStatusesAsync();
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region WorkflowTypes

        [HttpGet("workflow-types")]
        public async Task<IActionResult> GetWorkflowTypes()
        {
            var result = await _workflowLookupsService.GetWorkflowTypesAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("workflow-types/{id:guid}")]
        public async Task<IActionResult> GetWorkflowType(Guid id)
        {
            var result = await _workflowLookupsService.GetWorkflowTypeAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region WorkflowSteps
        [HttpGet("workflow-steps")]
        public async Task<IActionResult> GetWorkflowSteps()
        {
            var result = await _workflowLookupsService.GetWorkflowStepsAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("workflow-steps/{id:guid}")]
        public async Task<IActionResult> GetWorkflowSteps(Guid id)
        {
            var result = await _workflowLookupsService.GetWorkflowStepAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("workflow-types/{workflowTypeId:guid}/steps")]
        public async Task<IActionResult> GetWorkflowStepsByWorkflowType(Guid workflowTypeId)
        {
            var result = await _workflowLookupsService.GetWorkflowStepsAsync(workflowTypeId);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region WorkflowAction

        [HttpGet("workflow-actions")]
        public async Task<IActionResult> GetWorkflowActions()
        {
            var result = await _workflowLookupsService.GetWorkflowActionsAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("workflow-actions/{id:guid}")]
        public async Task<IActionResult> GetWorkflowActions(Guid id)
        {
            var result = await _workflowLookupsService.GetWorkflowActionAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("workflow-types/{workflowTypeId:guid}/actions")]
        public async Task<IActionResult> GetWorkflowActionsByWorkflowTypeId(Guid workflowTypeId)
        {
            var result = await _workflowLookupsService.GetWorkflowActionsAsync(workflowTypeId);
            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region WorkflowStepActions
        [HttpGet("workflow-step-actions/{id:guid}")]
        public async Task<IActionResult> GetWorkflowStepActions(Guid id)
        {
            var result = await _workflowLookupsService.GetWorkflowStepActionAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("workflow-step-actions")]
        public async Task<IActionResult> GetWorkflowStepActionByWorkflowStep([FromQuery] Guid workflowStepId)
        {
            var result = await _workflowLookupsService.GetWorkflowStepActionsAsync(workflowStepId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("workflow-types/{workflowTypeId:guid}/step-actions")]
        public async Task<IActionResult> GetWorkflowStepActionsByWorkflowType(Guid workflowTypeId)
        {
            var result = await _workflowLookupsService.GetWorkflowStepActionsByWorkflowTypeAsync(workflowTypeId);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region WorkflowStepCategories
        [HttpGet("workflow-step-categories")]
        public IActionResult GetWorkflowStepCategories()
        {
            return Ok(EnumExtensions.GetValues<WorkflowStepsCategories>());
        }
        #endregion
    }
}
