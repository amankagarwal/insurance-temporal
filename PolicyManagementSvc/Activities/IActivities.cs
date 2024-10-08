using Common.Models;
using Temporalio.Activities;

namespace PolicyManagementSvc.Activities;

public interface IActivities
{
    [Activity]
    Task<string> SampleActivityAsync();
    [Activity]
    Task<PremiumResponse> GetPremiumAsync(PremiumRequest request);
    [Activity]
    Task<bool> ValidateUserInputsAsync(PolicyCreationRequest request);
}