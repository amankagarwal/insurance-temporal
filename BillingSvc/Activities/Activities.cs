using System.Text;
using Common.Models;
using Newtonsoft.Json;
using Temporalio.Activities;

namespace BillingSvc.Activities;

public class Activities : IActivities
{
    [Activity]
    public async Task<PaymentResponse> ProcessPaymentAsync(BillingRequest request)
    {
        Console.WriteLine($"Processing recurring payment of {request.Premium}");
        var response = await CallPaymentGateway(request, false);
        if (!response.Success)
        {
            throw new Exception("Failure processing payment");
        }

        return response;
    }

    
    /// <summary>
    ///     First payment also requires us to collect the Fees upfront
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Activity]
    public async Task<PaymentResponse> ProcessFirstPaymentAsync(BillingRequest request)
    {
        Console.WriteLine($"Processing payment of Premium:{request.Premium} and Fees:{request.Fees}");
        var response = await CallPaymentGateway(request, true);
        if (!response.Success)
        {
            throw new Exception("Failure processing payment");
        }
        return response;
    }

    private async Task<PaymentResponse> CallPaymentGateway(BillingRequest request, bool isFirstPayment)
    {
        var amount = isFirstPayment ? request.Premium + request.Fees : request.Premium;
        var paymentRequest = new PaymentRequest
        {
            Amount = amount,
            PaymentType = amount >= 0 ? PaymentType.CHARGE : PaymentType.REFUND
        };
        
        using var httpClient = new HttpClient();
        // Set the base address and configure the client
        httpClient.BaseAddress = new Uri("http://localhost:7001");

        // Serialize the request object into JSON
        var jsonRequest = JsonConvert.SerializeObject(paymentRequest);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Make the API call to the rate endpoint
        var response = await httpClient.PostAsync("/payment", content);

        // Ensure the response is successful
        response.EnsureSuccessStatusCode();

        // Read and deserialize the response content to PremiumResponse model
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(jsonResponse);

        return paymentResponse;
    }
}