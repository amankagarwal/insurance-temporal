using Common.Models;

namespace PolicyManagementSvc.Services;

public interface IWorkflowService
{
    public Task<PolicyCreationResponse> CreatePolicyWorkflow(PolicyCreationRequest request);
    public Task<string> SampleWorkflow();
}