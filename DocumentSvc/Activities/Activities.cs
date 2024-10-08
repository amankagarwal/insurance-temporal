using Common.Models;

namespace DocumentSvc.Activities;

public class Activities : IActivities
{
    public async Task SendCustomerEmailAsync(string template)
    {
        await Task.Delay(500);
        Console.WriteLine("Sent email to customer!");
    }

    public async Task<string> CreateTemplateAsync(DocumentRequest request)
    {
        await Task.Delay(500);
        return "Dummy Template";
    }
}