using System;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore;
using Hexure.EntityFrameworkCore.Events.Repositories;
using Hexure.Events.Serialization;
using Hexure.Results.Extensions;
using Hexure.Time;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Hexure.EventsPublisher
{
    public class EventsPublisher
    {
        private readonly int _defaultBatchSize;
        private readonly TimeSpan _defaultDelay;
        private readonly IBusControl _busControl;
        private readonly ISystemTimeProvider _systemTimeProvider;
        private readonly IEventDeserializer _eventDeserializer;
        private readonly IServiceProvider _serviceProvider;

        public EventsPublisher(int defaultBatchSize, TimeSpan defaultDelay, IServiceProvider serviceProvider)
        {
            _defaultBatchSize = defaultBatchSize;
            _defaultDelay = defaultDelay;

            _serviceProvider = serviceProvider;
            _busControl = serviceProvider.GetRequiredService<IBusControl>();
            _systemTimeProvider = serviceProvider.GetRequiredService<ISystemTimeProvider>();
            _eventDeserializer = serviceProvider.GetRequiredService<IEventDeserializer>();
        }

        public async Task RunAsync()
        {
            await _busControl.StartAsync();

            try
            {
                while (true)
                {
                    using (var scopedContainer = _serviceProvider.CreateScope())
                    {
                        var transactionProvider =
                            scopedContainer.ServiceProvider.GetRequiredService<ITransactionProvider>();
                        var serializedEventRepository = scopedContainer.ServiceProvider
                            .GetRequiredService<ISerializedEventRepository>();

                        await transactionProvider.BeginTransactionAsync();
                        Console.WriteLine($"{DateTime.UtcNow} Publishing (batch size: {_defaultBatchSize})...");
                        var eventsToPublish = await serializedEventRepository.GetUnpublishedEventsAsync(_defaultBatchSize);

                        foreach (var serializedEventEntity in eventsToPublish)
                        {
                            try
                            {
                                await _eventDeserializer.Deserialize(serializedEventEntity.SerializedEvent)
                                    .OnSuccess(async @event =>
                                    {
                                        await _busControl.Publish(@event);
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

                    await Task.Delay(_defaultDelay);
                }
            }
            catch
            {
                await _busControl.StopAsync();
            }
        }
    }
}