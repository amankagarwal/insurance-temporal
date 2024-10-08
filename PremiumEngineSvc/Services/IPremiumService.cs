using Common.Models;

namespace PremiumEngineSvc.Services;

public interface IPremiumService
{
    public Task<PremiumResponse> GetRateAsync(PremiumRequest request);
}