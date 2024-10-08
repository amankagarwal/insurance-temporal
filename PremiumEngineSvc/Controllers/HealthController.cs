using Microsoft.AspNetCore.Mvc;

namespace PremiumEngineSvc.Controllers;

public class HealthController : ControllerBase
{
    
    /// <summary>
    ///     Simple health check to make sure the service is running
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("health")]
    public async Task<IActionResult> Index()
    {
        return Ok("Service is healthy");
    }
}