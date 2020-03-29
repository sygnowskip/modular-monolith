using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularMonolith.Registrations.Commands;
using ModularMonolith.Registrations.Contracts;
using ModularMonolith.Time;

namespace ModularMonolith.Registrations.ApplicationServices
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRegistrationServices(this IServiceCollection services)
        {
            services.AddTransient<IRegistrationApplicationService, RegistrationApplicationService>();
            services.AddMediatR(typeof(CreateRegistrationCommandHandler).Assembly);
            
            services.TryAddTransient<ISystemTimeProvider, SystemTimeProvider>();
            return services;
        }
    }
}