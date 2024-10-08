using System.Text;
using System.Text.Json;
using Common.Models;

namespace ClientSvc;

class Program
{
    private static readonly HttpClient client = new();
    private static readonly Random random = new();

    static async Task Main(string[] args)
    {
        var url = "http://localhost:3001/policy";
        var policyCounter = 1;
        var endTime = DateTime.Now.AddSeconds(60); // Run the loop for 60 seconds

        while (DateTime.Now < endTime)
        {
            // Generate policy data
            var policyRequest = GeneratePolicyRequest(policyCounter);

            // Serialize the request payload
            var jsonContent = new StringContent(JsonSerializer.Serialize(policyRequest), Encoding.UTF8, "application/json");

            try
            {
                // Send the POST request
                var response = await client.PostAsync(url, jsonContent);

                // Check the response
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Policy {policyCounter} created successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to create policy {policyCounter}. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while creating policy {policyCounter}: {ex.Message}");
            }

            policyCounter++;
        }
    }

    // Method to generate a new PolicyCreationRequest
    private static PolicyCreationRequest GeneratePolicyRequest(int policyId)
    {
        return new PolicyCreationRequest
        {
            PolicyId = $"{policyId}",
            VIN = GenerateRandomVIN(),
            VehicleId = Guid.NewGuid().ToString(),
            Coverages = GenerateRandomCoverages()
        };
    }

    // Generate a random 17-character VIN
    private static string GenerateRandomVIN()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var vin = new char[17];

        for (var i = 0; i < 17; i++)
        {
            vin[i] = chars[random.Next(chars.Length)];
        }

        return new string(vin);
    }

    // Generate a random number of Coverages between 1 and 10
    private static List<Coverage> GenerateRandomCoverages()
    {
        var numCoverages = random.Next(1, 11); // Random number between 1 and 10
        var coverages = new List<Coverage>();

        for (var i = 0; i < numCoverages; i++)
        {
            coverages.Add(new Coverage
            {
                CoverageName = "SAMPLE_COVERAGE_NAME",
                CoverageValue = random.Next(1000, 5000).ToString()
            });
        }

        return coverages;
    }
}