using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.Workers
{
    public abstract class HealthCheckEndpointStartup
    {
        public abstract void ConfigureServices(IServiceCollection serviceCollection);

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/health-check");
            });
        }
    }
}