using Common.Models;
using Temporalio.Activities;

namespace DocumentSvc.Activities;

public interface IActivities
{
    [Activity]
    public Task<string> CreateTemplateAsync(DocumentRequest request);
    
    [Activity]
    public Task SendCustomerEmailAsync(string template);
}