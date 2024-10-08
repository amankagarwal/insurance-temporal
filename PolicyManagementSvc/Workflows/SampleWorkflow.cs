using PolicyManagementSvc.Activities;
using Temporalio.Workflows;

namespace PolicyManagementSvc.Workflows;

[Workflow]
public class SampleWorkflow
{
    [WorkflowRun]
    public async Task<string> SampleWorkflowRun()
    {
        try
        {
            var sampleActivity = await Workflow.ExecuteActivityAsync(
                (IActivities act) => act.SampleActivityAsync(),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5) }
            );

            return "Workflow completed!";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating policy: {ex}");
            return "Failed Creating Policy";
        }
    }
}