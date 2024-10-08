namespace Common.Models;

public class PaymentRequest
{
    public PaymentType PaymentType { get; set; }
    public decimal Amount { get; set; }
}