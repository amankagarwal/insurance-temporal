using Common.Models;
using Microsoft.AspNetCore.Mvc;
using PremiumEngineSvc.Services;

namespace PremiumEngineSvc.Controllers;

public class PremiumController : ControllerBase
{
    private readonly IPremiumService _premiumService;

    public PremiumController(IPremiumService premiumService)
    {
        _premiumService = premiumService;
    }

    /// <summary>
    ///     A simple rate controller to return premiums
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("rate")]
    public async Task<IActionResult> GetRate([FromBody] PremiumRequest request)
    {
        return Ok(await _premiumService.GetRateAsync(request));
    }
}