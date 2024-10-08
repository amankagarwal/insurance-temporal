using PolicyManagementSvc.Activities;
using PolicyManagementSvc.Services;
using PolicyManagementSvc.Workers;
using Temporalio.Client;
using WorkflowService = PolicyManagementSvc.Services.WorkflowService;

namespace PolicyManagementSvc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC controllers
            services.AddControllers(); 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Register the Temporal client as a Singleton
            services.AddSingleton<ITemporalClient>( sp => TemporalClient
                .ConnectAsync(new("localhost:7233"))
                .GetAwaiter()
                .GetResult());

            services.AddTransient<IActivities, Activities.Activities>();
            // Register the Temporal worker as a Hosted Service
            services.AddHostedService<TemporalWorkerService>();
            services.AddTransient<IWorkflowService, WorkflowService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            // Map controller endpoints
            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
            });
        }
    }
}