using Common.Models;

namespace PaymentGatewaySvc.Services;

public class PaymentService : IPaymentService
{
    public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
    {
        if (request.PaymentType == PaymentType.CHARGE)
            Console.WriteLine("Payment charge processed");
        else
            Console.WriteLine("Payment refund processed");
        return new PaymentResponse
        {
            Success = true
        };
    }
}