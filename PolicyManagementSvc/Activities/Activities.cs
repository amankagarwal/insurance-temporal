using System.Text;
using Common.Models;
using Newtonsoft.Json;
using Temporalio.Activities;

namespace PolicyManagementSvc.Activities;

public class Activities : IActivities
{
    private readonly ILogger<Activities> _logger;
    private readonly string PREMIUM_BASE_URL = "http://localhost:4001";

    public Activities(ILogger<Activities> logger)
    {
        _logger = logger;
    }

    [Activity]
    public async Task<string> SampleActivityAsync()
    {
        _logger.LogInformation("Performing activity...");
        // Simulate some async work
        await Task.Delay(500);
        return "Activity completed!";
    }
    
    [Activity]
    public async Task<PremiumResponse> GetPremiumAsync(PremiumRequest request)
    {
        using var httpClient = new HttpClient();
        // Set the base address and configure the client
        httpClient.BaseAddress = new Uri(PREMIUM_BASE_URL);

        // Serialize the request object into JSON
        var jsonRequest = JsonConvert.SerializeObject(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        // Make the API call to the rate endpoint
        var response = await httpClient.PostAsync("/rate", content);

        // Ensure the response is successful
        response.EnsureSuccessStatusCode();

        // Read and deserialize the response content to PremiumResponse model
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var premiumResponse = JsonConvert.DeserializeObject<PremiumResponse>(jsonResponse);

        return premiumResponse;
    }

    [Activity]
    public async Task<bool> ValidateUserInputsAsync(PolicyCreationRequest request)
    {
        await Task.Delay(500);
        return true;
    }
}