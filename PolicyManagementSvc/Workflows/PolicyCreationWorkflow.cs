using Common.Models;
using Microsoft.AspNetCore.Mvc;
using PolicyManagementSvc.Activities;
using Temporalio.Common;
using Temporalio.Workflows;

namespace PolicyManagementSvc.Workflows;

[Workflow]
public class PolicyCreationWorkflow
{
    private bool _initialPaymentCompleted;

    // Signal method to receive a signal from the Billing Workflow
    [WorkflowSignal]
    public Task InitialPaymentSuccessSignal()
    {
        Workflow.Logger.LogInformation("Signal received: Initial Payment Success");
        _initialPaymentCompleted = true;
        
        return Task.CompletedTask;
    }

    [WorkflowRun]
    public async Task<PolicyCreationResponse> PolicyCreationWorkflowRun([FromBody] PolicyCreationRequest request)
    {
        var response = new PolicyCreationResponse
        {
            Success = false,
            PolicyId = request.PolicyId
        };

        try
        {
            var workflowId = Workflow.Info.WorkflowId;

            var validateInputsResponse = await Workflow.ExecuteActivityAsync(
                (IActivities act) => act.ValidateUserInputsAsync(request),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(5) }
            );

            if (!validateInputsResponse)
            {
                response.Error = "Validation Failed";
                return response;
            }

            var premiumRequest = new PremiumRequest
            {
                Coverages = request.Coverages,
                VehicleId = request.VehicleId,
                VIN = request.VIN
            };

            var premiumResponse = await Workflow.ExecuteActivityAsync(
                (IActivities act) => act.GetPremiumAsync(premiumRequest),
                new ActivityOptions
                {
                    StartToCloseTimeout = TimeSpan.FromMinutes(5),
                    RetryPolicy = new RetryPolicy
                    {
                        InitialInterval = TimeSpan.FromSeconds(2),
                        BackoffCoefficient = 2.0f,
                        MaximumInterval = TimeSpan.FromSeconds(30),
                        MaximumAttempts = 5
                    }
                }
            );

            var billingRequest = new BillingRequest
            {
                Premium = premiumResponse.Premium,
                Fees = premiumResponse.Fees,
                NumberOfMonths = 6,
                ParentWorkflowId = workflowId
            };

            // Start the billing child workflow asynchronously
            _ = await Workflow.StartChildWorkflowAsync(
                "BillingWorkflow",
                new object[] { billingRequest },
                new ChildWorkflowOptions
                {
                    TaskQueue = TaskQueue.POLICY_BILLING.ToString(),
                    Id = $"billing-{request.PolicyId}",
                    ParentClosePolicy = ParentClosePolicy.Abandon
                }
            );

            // Wait for the signal from the Billing Workflow
            Workflow.Logger.LogInformation("Waiting for initial payment completion signal...");

            await Workflow.WaitConditionAsync(
                () => _initialPaymentCompleted,
                TimeSpan.FromMinutes(10) // This is to ensure the workflow doesnt wait on the signal forever
            );

            Workflow.Logger.LogInformation("Initial payment completed, proceeding to document workflow...");

            // Start the document child workflow asynchronously
            var documentRequest = new DocumentRequest
            {
                ParentWorkflowId = workflowId,
                PolicyId = request.PolicyId
            };

            _ = await Workflow.StartChildWorkflowAsync(
                "DocumentWorkflow",
                new object[] { documentRequest },
                new ChildWorkflowOptions
                {
                    TaskQueue = TaskQueue.POLICY_DOCUMENT.ToString(),
                    Id = $"document-{request.PolicyId}",
                    ParentClosePolicy = ParentClosePolicy.Abandon
                }
            );

            response.Success = true;
            response.Message = "Policy Creation Successful";
            return response;

        }
        catch (Exception ex)
        {
            response.Error = $"Exception creating policy: {ex.Message}";
            return response;
        }
    }
}
