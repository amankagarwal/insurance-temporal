using BillingSvc.Activities;
using BillingSvc.Workflows;
using Common.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Temporalio.Client;
using Temporalio.Worker;

namespace BillingSvc.Workers
{
    public class TemporalWorkerService : BackgroundService
    {
        private readonly ILogger<TemporalWorkerService> _logger;
        private readonly ITemporalClient _client;
        private readonly IActivities _activities;


        public TemporalWorkerService(
            ILogger<TemporalWorkerService> logger,
            ITemporalClient client,
            IActivities activities)
        {
            _logger = logger;
            _client = client;
            _activities = activities;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting Temporal worker...");

            try
            {
                // Start the Temporal worker with injected client and activities
                using var worker = new TemporalWorker(
                    _client,
                    new TemporalWorkerOptions(taskQueue: TaskQueue.POLICY_BILLING.ToString())
                        .AddAllActivities(_activities)
                        .AddWorkflow<BillingWorkflow>());

                await worker.ExecuteAsync(stoppingToken);  // Run the worker

                _logger.LogInformation("Temporal worker started successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while running the Temporal worker.");
                throw;
            }
        }
    }
}