namespace Common.Models;

public class BillingRequest
{
    public decimal Premium { get; set; }
    public decimal Fees { get; set; }
    public int NumberOfMonths { get; set; }
    public int PolicyId { get; set; }
    public string ParentWorkflowId { get; set; }
}