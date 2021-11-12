using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hexure.Workers
{
    public abstract class HealthCheckEndpointStartup
    {
        public abstract void ConfigureServices(IServiceCollection serviceCollection);

        public virtual void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapHealthChecks("/api/health-check"); });
        }
    }
}