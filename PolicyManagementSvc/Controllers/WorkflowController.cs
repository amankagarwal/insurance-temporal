using Common.Models;
using Microsoft.AspNetCore.Mvc;
using PolicyManagementSvc.Services;

namespace PolicyManagementSvc.Controllers;

public class WorkflowController : ControllerBase
{
    private readonly IWorkflowService _workflowService;

    public WorkflowController(IWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }
    
    /// <summary>
    ///     Create a sample workflow
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("sample")]
    public async Task<IActionResult> CreateSampleWorkflow()
    {
        return Ok(await _workflowService.SampleWorkflow());
    }

    /// <summary>
    ///     Create a new policy for a customer
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("policy")]
    public async Task<IActionResult> CreateNewPolicy([FromBody] PolicyCreationRequest request)
    {
        return Ok(await _workflowService.CreatePolicyWorkflow(request));
    }
}