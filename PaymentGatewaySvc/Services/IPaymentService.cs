using Common.Models;

namespace PaymentGatewaySvc.Services;

public interface IPaymentService
{
    public Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
}