using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PremiumEngineSvc.Services;

namespace PremiumEngineSvc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVC controllers
            services.AddControllers();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IPremiumService, PremiumService>();
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