namespace Common.Models;

public class PolicyCreationRequest
{
    public string PolicyId { get; set; }
    public string VehicleId { get; set; }
    public string VIN { get; set; }
    public List<Coverage> Coverages {get; set; }
}