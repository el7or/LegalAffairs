using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos.Workflow;
using Moe.La.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers.Workflow
{
    [Route("api/workflow-configuration")]
    [ApiController]
    public class WorkflowConfigurationController : ControllerBase
    {
        private readonly IWorkflowConfigurationService _workflowConfigurationService;

        public WorkflowConfigurationController(IWorkflowConfigurationService workflowConfigurationService)
        {
            _workflowConfigurationService = workflowConfigurationService;
        }

        #region WorkflowTypes

        [HttpPost("add-workflow-type")]
        public async Task<IActionResult> AddWorkflowType([FromBody] WorkflowTypeDto workflowType)
        {
            var result = await _workflowConfigurationService.AddWorkflowTypeAsync(workflowType);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("update-workflow-type")]
        public async Task<IActionResult> UpdateWorkflowType([FromBody] WorkflowTypeDto workflowType)
        {
            var result = await _workflowConfigurationService.EditWorkflowTypeAsync(workflowType);
            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region WorkflowSteps
        [HttpPost("add-workflow-step")]
        public async Task<IActionResult> AddWorkflowStep([FromBody] WorkflowStepDto workflowStep)
        {
            var result = await _workflowConfigurationService.AddWorkflowStepAsync(workflowStep);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("update-workflow-step")]
        public async Task<IActionResult> UpdateWorkflowStep([FromBody] WorkflowStepDto workflowStep)
        {
            var result = await _workflowConfigurationService.EditWorkflowStepAsync(workflowStep);
            return StatusCode((int)result.StatusCode, result);
        }

        #endregion

        #region WorkflowActions
        [HttpPost("add-workflow-action")]
        public async Task<IActionResult> AddWorkflowAction([FromBody] WorkflowActionDto workflowAction)
        {
            var result = await _workflowConfigurationService.AddWorkflowActionAsync(workflowAction);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("update-workflow-action")]
        public async Task<IActionResult> UpdateWorkflowAction([FromBody] WorkflowActionDto workflowAction)
        {
            var result = await _workflowConfigurationService.EditWorkflowActionAsync(workflowAction);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region WorkflowStepActions
        [HttpPost("add-workflow-step-action")]
        public async Task<IActionResult> AddWorkflowStepAction([FromBody] WorkflowStepActionDto workflowStepAction)
        {
            var result = await _workflowConfigurationService.AddWorkflowStepActionAsync(workflowStepAction);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("update-workflow-step-action")]
        public async Task<IActionResult> UpdateWorkflowStepAction([FromBody] WorkflowStepActionDto workflowStepAction)
        {
            var result = await _workflowConfigurationService.UpdateWorkflowStepActionAsync(workflowStepAction);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion

        #region WorkflowStepPermissions
        [HttpPost("add-workflow-step-permission")]
        public async Task<IActionResult> AddWorkflowStepPermission([FromBody] WorkflowStepPermissionDto workflowStepPermission)
        {
            var result = await _workflowConfigurationService.AddWorkflowStepPermissionAsync(workflowStepPermission);
            return StatusCode((int)result.StatusCode, result);
        }
        #endregion
    }
}
