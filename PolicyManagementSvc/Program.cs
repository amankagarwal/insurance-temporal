using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace PolicyManagementSvc
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Build configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
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
                Log.Information("Starting web host");

                // Use Host.CreateDefaultBuilder for .NET 6+
                var host = Host.CreateDefaultBuilder(args)
                    .UseSerilog()  // Add Serilog to the logging pipeline
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                        webBuilder.UseConfiguration(config);
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
