using Common.Models;

namespace PremiumEngineSvc.Services;

public class PremiumService : IPremiumService
{

    private readonly decimal BASE_RATE = 1000;
    /// <summary>
    ///     Add some random, deterministic premium calculation
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<PremiumResponse> GetRateAsync(PremiumRequest request)
    {
        if (request.Coverages.Count == 0)
            throw new ArgumentException("You must pass coverages to calculate a premium!");
        var response = new PremiumResponse
        {
            // A random, deterministic premium calculation for simplicity
            Premium = BASE_RATE * request.Coverages.Count,
            // A constant fees for simplicity
            Fees = 10
        };
        return await Task.FromResult(response);
    }
}