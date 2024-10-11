using BillingSvc.Activities;
using Common.Models;
using Temporalio.Workflows;

namespace BillingSvc.Workflows;

[Workflow]
public class BillingWorkflow
{
    [WorkflowRun]
    public async Task<BillingResponse> RunAsync(BillingRequest request)
    {
        await Workflow.ExecuteActivityAsync(
            (IActivities act) => act.ProcessFirstPaymentAsync(request),
            new ActivityOptions
            {
                TaskQueue = TaskQueue.POLICY_BILLING.ToString(),
                StartToCloseTimeout = TimeSpan.FromMinutes(5)
            }
        );

        // Send external signal to tell parent workflow that initial payment was successful
        var parentWorkflowHandle = Workflow.GetExternalWorkflowHandle(request.ParentWorkflowId);

        await parentWorkflowHandle.SignalAsync(BillingToPolicySignals.INITIAL_PAYMENT_SUCCESS_SIGNAL, Array.Empty<object>());

        // For the purposes of the demo, keep making additional charges every 10s for 1 mins
        // This mimic 6 monthly payments
        var endTime = Workflow.UtcNow.Add(TimeSpan.FromMinutes(1));
        while (Workflow.UtcNow < endTime)
        {
            // Wait for 10 seconds
            await Workflow.DelayAsync(TimeSpan.FromSeconds(10));

            // Charge the customer
            await Workflow.ExecuteActivityAsync(
                (IActivities act) => act.ProcessPaymentAsync(request),
                new ActivityOptions
                {
                    TaskQueue = TaskQueue.POLICY_BILLING.ToString(),
                    StartToCloseTimeout = TimeSpan.FromMinutes(5)
                }
            );
        }

        // Optionally return a final BillingResponse if needed
        return new BillingResponse { Success = true };
    }
}
