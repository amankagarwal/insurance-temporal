using Common.Models;
using Microsoft.AspNetCore.Mvc;
using PaymentGatewaySvc.Services;

namespace PaymentGatewaySvc.Controllers;

public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    ///     A simple rate controller to return premiums
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("payment")]
    public async Task<IActionResult> ProcessPaymentAsync([FromBody] PaymentRequest request)
    {
        return Ok(await _paymentService.ProcessPaymentAsync(request));
    }
}