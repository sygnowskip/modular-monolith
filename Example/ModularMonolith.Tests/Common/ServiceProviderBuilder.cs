﻿using System;
using Hexure.Events;
using Hexure.Events.Namespace;
using Hexure.MassTransit.Events.Builders;
using Hexure.MassTransit.RabbitMq;
using Hexure.MassTransit.RabbitMq.Settings;
using Hexure.Testing.Logging;
using Hexure.Testing.Resources;
using Hexure.Testing.Snapshots;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using ModularMonolith.Configuration;
using ModularMonolith.Tests.Scenarios;

namespace ModularMonolith.Tests.Common
{
    public class ServiceProviderBuilder
    {
        public static IServiceProvider Build()
        {
            var applicationSettings = ApplicationSettingsConfigurationProvider.Get();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder => builder.AddNUnitConsoleOutput());
            
            serviceCollection.AddSingleton<IConfiguration>(provider => applicationSettings);
            serviceCollection.AddTransient(serviceProvider =>
                serviceProvider.GetRequiredService<IConfiguration>().GetSection(ApplicationSettings.Authority).Get<AuthoritySettings>());
            serviceCollection.AddTransient(serviceProvider =>
                serviceProvider.GetRequiredService<IConfiguration>().GetSection(ApplicationSettings.Monolith).Get<MonolithApiSettings>());
            serviceCollection.AddTransient<IHttpClientProvider, HttpClientProvider>();
            
            serviceCollection.AddTransient<Scenarios.Scenarios>();
            serviceCollection.AddTransient<ExamScenarios>();
            serviceCollection.AddTransient<LocationScenarios>();
            serviceCollection.AddTransient<MessageScenarios>();
            serviceCollection.AddTransient<OrderScenarios>();
            serviceCollection.AddTransient<RegistrationScenarios>();
            serviceCollection.AddTransient<SubjectScenarios>();

            serviceCollection.AddHttpClient();
            serviceCollection.AddDomainEvents();
            serviceCollection.AddEventTypeProvider(new ConsumersEventTypeProviderBuilder(new EventNamespaceReader())
                .Build());
            serviceCollection.RegisterRabbitMqPublisher(applicationSettings.GetSection(ApplicationSettings.Bus)
                .Get<PublisherRabbitMqSettings>());
            
            serviceCollection.AddSnapshots(ApplicationSettings.ConnectionStrings.Database);
            serviceCollection.AddResourceAwaiters(applicationSettings.GetSection(ApplicationSettings.Bus)
                .Get<RabbitMqResourceConfiguration>());

            return serviceCollection.BuildServiceProvider();
        }
    }
}