using System;
using System.Threading;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore;
using Hexure.EntityFrameworkCore.Events.Repositories;
using Hexure.Events.Serialization;
using Hexure.Results.Extensions;
using Hexure.Time;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Hexure.EventsPublisher
{
    public class EventsPublisher
    {
        private readonly EventsPublisherSettings _settings;
        private readonly IBusControl _busControl;
        private readonly ISystemTimeProvider _systemTimeProvider;
        private readonly IEventDeserializer _eventDeserializer;
        private readonly IServiceProvider _serviceProvider;

        public EventsPublisher(EventsPublisherSettings settings, IBusControl busControl,
            ISystemTimeProvider systemTimeProvider, IEventDeserializer eventDeserializer,
            IServiceProvider serviceProvider)
        {
            _settings = settings;
            _busControl = busControl;
            _systemTimeProvider = systemTimeProvider;
            _eventDeserializer = eventDeserializer;
            _serviceProvider = serviceProvider;
        }

        public async Task RunAsync(CancellationToken stoppingToken)
        {
            await _busControl.StartAsync(stoppingToken);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        using (var scopedContainer = _serviceProvider.CreateScope())
                        {
                            var transactionProvider =
                                scopedContainer.ServiceProvider.GetRequiredService<ITransactionProvider>();
                            var serializedEventRepository = scopedContainer.ServiceProvider
                                .GetRequiredService<ISerializedEventRepository>();

                            await transactionProvider.BeginTransactionAsync();
                            Console.WriteLine($"{DateTime.UtcNow} Publishing (batch size: {_settings.BatchSize})...");
                            var eventsToPublish =
                                await serializedEventRepository.GetUnpublishedEventsAsync(_settings.BatchSize);

                            foreach (var serializedEventEntity in eventsToPublish)
                            {
                                try
                                {
                                    await _eventDeserializer.Deserialize(serializedEventEntity.SerializedEvent)
                                        .OnSuccess(async @event =>
                                        {
                                            await _busControl.Publish(@event, stoppingToken);
                                        });

                                    await serializedEventEntity.MarkAsProcessed(_systemTimeProvider.UtcNow)
                                        .OnSuccess(() => serializedEventRepository.SaveChangesAsync())
                                        .OnFailure(errors =>
                                            throw new InvalidOperationException(JsonConvert.SerializeObject(errors)));
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.ToString());
                                }
                            }

                            await transactionProvider.CommitTransactionAsync();
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.ToString());
                        //TODO: Logging
                    }
                    finally
                    {
                        await Task.Delay(_settings.Delay, stoppingToken);
                    }
                }
            }
            catch
            {
                await _busControl.StopAsync(stoppingToken);
            }
        }
    }
}