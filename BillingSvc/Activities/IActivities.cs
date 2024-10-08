using Common.Models;
using Temporalio.Activities;

namespace BillingSvc.Activities;

public interface IActivities
{
    [Activity]
    public Task<PaymentResponse> ProcessPaymentAsync(BillingRequest request);
    [Activity]
    public Task<PaymentResponse> ProcessFirstPaymentAsync(BillingRequest request);
}