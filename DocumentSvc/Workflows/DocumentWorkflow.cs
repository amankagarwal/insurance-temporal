using Common.Models;
using DocumentSvc.Activities;
using Temporalio.Workflows;

namespace DocumentSvc.Workflows;

[Workflow]
public class DocumentWorkflow
{
    [WorkflowRun]
    public async Task DocumentWorkflowAsync(DocumentRequest request)
    {
        var template = await Workflow.ExecuteActivityAsync(
            (IActivities act) => act.CreateTemplateAsync(request),
            new ActivityOptions
            {
                TaskQueue = TaskQueue.POLICY_DOCUMENT.ToString(),
                StartToCloseTimeout = TimeSpan.FromMinutes(5)
            }
        );

        await Workflow.ExecuteActivityAsync(
            (IActivities act) => act.SendCustomerEmailAsync(template),
            new ActivityOptions
            {
                TaskQueue = TaskQueue.POLICY_DOCUMENT.ToString(),
                StartToCloseTimeout = TimeSpan.FromMinutes(5)
            }
        );
    }
}