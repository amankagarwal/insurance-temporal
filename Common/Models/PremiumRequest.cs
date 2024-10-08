namespace Common.Models;

public class PremiumRequest
{
    public string VehicleId { get; set; }
    public string VIN { get; set; }
    public List<Coverage> Coverages {get; set; }
}

public class Coverage
{
    public string CoverageName { get; set; }
    public string CoverageValue { get; set; }
}