using System;
using Hexure.MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using ModularMonolith.API.Settings;
using ModularMonolith.Dependencies;

namespace ModularMonolith.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authoritySettings = Configuration.GetSection("Authority").Get<AuthoritySettings>();
            
            services
                .AddControllers()
                .AddNewtonsoftJson();

            
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = authoritySettings.Url;
                    options.RequireHttpsMetadata = false;

                    options.Audience = "modular-monolith";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Registrations",
                    policy => policy.RequireClaim("scope", "registrations"));

                options.AddPolicy("Payments",
                    policy => policy.RequireClaim("scope", "payments"));
            });

            IdentityModelEventSource.ShowPII = true;
            
            services.AddSwaggerDocument(settings =>
            {
                settings.Title =  "Modular monolith API";
            });

            services.AddCommandServices();
            services.AddQueryServices();
            services.AddHealthChecks()
                .AddSqlServer(Configuration.GetConnectionString("Database"))
                .AddIdentityServer(new Uri(authoritySettings.Url));
            
            //TODO: Clean up
            services.AddRegistrations();
            services.AddPayments();
            services.AddPersistence(Configuration.GetConnectionString("Database"))
                .AddTransactionalCommands();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/api/health-check");
            });
        }
    }
}
