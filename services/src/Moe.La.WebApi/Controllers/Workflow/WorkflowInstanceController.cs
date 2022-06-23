using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moe.La.Core.Dtos.Workflow;
using Moe.La.Core.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace Moe.La.WebApi.Controllers.Workflow
{
    [Route("api/workflow-instances")]
    [ApiController]
    public class WorkflowInstanceController : ControllerBase
    {
        private readonly IWorkflowInstanceService _workflowInstanceService;

        public WorkflowInstanceController(IWorkflowInstanceService workflowInstanceService)
        {
            _workflowInstanceService = workflowInstanceService;
        }

        [HttpPost("initiate")]
        public async Task<IActionResult> InitiateWorkflowInstance([FromBody] WorkflowInitiatorDto workflowInitiator)
        {
            // TODO: Move the logic to the service.
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest); // TODO: add a propriate error message.
            }

            var workflowInstance = new WorkflowInstanceDto
            {
                WorkflowTypeId = workflowInitiator.WorkflowTypeId,
            };

            var result = await _workflowInstanceService.AddWorkflowInstanceAsync(workflowInstance, workflowInitiator.WorkflowActionId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessWorkflowInstance(WorkflowProcessDto workflowProcess)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest); // TODO: add a propriate error message.
            }

            var result = await _workflowInstanceService.ProcessWorkflowInstanceAsync(workflowProcess.WorkflowInstanceId, workflowProcess.WorkflowActionId, workflowProcess.ProcessNote, workflowProcess.AssignedTo);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{workflowInstanceId:guid}")]
        public async Task<IActionResult> GetWorkflowInstance(Guid workflowInstanceId)
        {
            var result = await _workflowInstanceService.GetWorkflowInstanceAsync(workflowInstanceId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkflowInstances([FromQuery] Guid workflowTypeId, [FromQuery] Guid? workflowStepId = null, [FromQuery] int? workflowStatusId = null)
        {
            var result = await _workflowInstanceService.GetWorkflowInstancesAsync(workflowTypeId, workflowStepId, workflowStatusId);
            return StatusCode((int)result.StatusCode, result);
        }


    }
}
