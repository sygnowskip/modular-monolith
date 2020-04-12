using System;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore;
using Hexure.EntityFrameworkCore.Events.Repositories;
using Hexure.Results.Extensions;
using Hexure.Time;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Hexure.EventsPublisher
{
    public class EventsPublisher
    {
        private readonly int _defaultBatchSize;
        private readonly TimeSpan _defaultDelay;
        private readonly IMediator _mediator;
        private readonly ISerializedEventRepository _serializedEventRepository;
        private readonly ISystemTimeProvider _systemTimeProvider;
        private readonly ITransactionProvider _transactionProvider;

        public EventsPublisher(int defaultBatchSize, TimeSpan defaultDelay, IServiceProvider serviceProvider)
        {
            _serializedEventRepository = serviceProvider.GetRequiredService<ISerializedEventRepository>();
            _mediator = serviceProvider.GetRequiredService<IMediator>();
            _systemTimeProvider = serviceProvider.GetRequiredService<ISystemTimeProvider>();
            _transactionProvider = serviceProvider.GetRequiredService<ITransactionProvider>();
            _defaultBatchSize = defaultBatchSize;
            _defaultDelay = defaultDelay;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                await _transactionProvider.BeginTransaction();
                Console.WriteLine($"{DateTime.UtcNow} Publishing (batch size: {_defaultBatchSize})...");
                var eventsToPublish = await _serializedEventRepository.GetUnpublishedEventsAsync(_defaultBatchSize);

                foreach (var serializedEventEntity in eventsToPublish)
                {
                    try
                    {
                        await _mediator.Publish(serializedEventEntity);

                        await serializedEventEntity.MarkAsProcessed(_systemTimeProvider.UtcNow)
                            .OnSuccess(() => _serializedEventRepository.SaveChangesASync())
                            .OnFailure(errors =>
                                throw new InvalidOperationException(JsonConvert.SerializeObject(errors)));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                await _transactionProvider.CommitTransaction();
                await Task.Delay(_defaultDelay);
            }
        }
    }
}