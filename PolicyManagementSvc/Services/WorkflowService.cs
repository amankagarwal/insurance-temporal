using Common.Models;
using PolicyManagementSvc.Workflows;
using Temporalio.Client;

namespace PolicyManagementSvc.Services;

public class WorkflowService : IWorkflowService
{
    private readonly ITemporalClient _client;

    public WorkflowService(ITemporalClient client)
    {
        _client = client;
    }

    public async Task<PolicyCreationResponse> CreatePolicyWorkflow(PolicyCreationRequest request)
    {
        var workflowId = $"create-policy-{request.PolicyId}";

        var handle = await _client.StartWorkflowAsync(
            (PolicyCreationWorkflow wf) => wf.PolicyCreationWorkflowRun(request),
            new(id: workflowId, taskQueue: TaskQueue.POLICY.ToString()));

        // Await the result of the workflow
        return await handle.GetResultAsync();
    }

    public async Task<string> SampleWorkflow()
    {
        var workflowId = $"sample-{Guid.NewGuid()}";

        var handle = await _client.StartWorkflowAsync(
            (SampleWorkflow wf) => wf.SampleWorkflowRun(),
            new(id: workflowId, taskQueue: TaskQueue.POLICY.ToString()));

        // Await the result of the workflow
        return await handle.GetResultAsync();
    }
}