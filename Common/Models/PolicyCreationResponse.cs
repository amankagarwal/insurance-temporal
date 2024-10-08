namespace Common.Models;

public class PolicyCreationResponse
{
    public string Error { get; set; }
    public string PolicyId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
}