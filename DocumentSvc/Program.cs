using DocumentSvc.Activities;
using DocumentSvc.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Temporalio.Client;

namespace DocumentSvc
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Build configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .Build();

            Console.WriteLine("Program started");

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                .CreateLogger();

            try
            {
                Log.Information("Starting worker host");

                // Create a generic host for running background services (workers)
                var host = Host.CreateDefaultBuilder(args)
                    .UseSerilog()  // Add Serilog to the logging pipeline
                    .ConfigureServices((hostContext, services) =>
                    {
                        // Register the Temporal client as a Singleton
                        services.AddSingleton<ITemporalClient>(sp => TemporalClient
                            .ConnectAsync(new("localhost:7233"))
                            .GetAwaiter()
                            .GetResult());

                        // Register activities and workers
                        services.AddTransient<IActivities, Activities.Activities>();
                        services.AddHostedService<TemporalWorkerService>();
                    })
                    .Build();

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                await Log.CloseAndFlushAsync();
            }
        }
    }
}
