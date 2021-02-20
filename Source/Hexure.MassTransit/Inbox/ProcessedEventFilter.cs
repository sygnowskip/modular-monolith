using System;
using System.Threading.Tasks;
using GreenPipes;
using Hexure.EntityFrameworkCore.Inbox.Entities;
using Hexure.EntityFrameworkCore.Inbox.Repositories;
using Hexure.EntityFrameworkCore.SqlServer.Inbox;
using Hexure.Time;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Hexure.MassTransit.Inbox
{
    public class ProcessedEventFilter<TMessage> : IFilter<ConsumeContext<TMessage>>
        where TMessage : class
    {
        private readonly IProcessedEventRepository _processedEventRepository;
        private readonly IConsumerProvider _consumerProvider;
        private readonly ISystemTimeProvider _systemTimeProvider;

        public ProcessedEventFilter(IProcessedEventRepository processedEventRepository,
            IConsumerProvider consumerProvider, ISystemTimeProvider systemTimeProvider)
        {
            _processedEventRepository = processedEventRepository;
            _consumerProvider = consumerProvider;
            _systemTimeProvider = systemTimeProvider;
        }

        public async Task Send(ConsumeContext<TMessage> context, IPipe<ConsumeContext<TMessage>> next)
        {
            try
            {
                var consumer = _consumerProvider.GetConsumer(context);
                if (consumer.HasNoValue || !context.MessageId.HasValue)
                    throw new InvalidOperationException("Unable to process message without identifier or consumer");
                
                var alreadyProcessed = await _processedEventRepository.ExistsAsync(context.MessageId.Value, consumer.Value);
                if (alreadyProcessed)
                {
                    MessageAlreadyProcessed(context.MessageId.Value, consumer.Value);
                    return;
                }
                
                await next.Send(context);
                
                await _processedEventRepository.AddAsync(
                    ProcessedEventEntity.Create(context.MessageId.Value, consumer.Value, _systemTimeProvider));
            }
            catch (DbUpdateException dbUpdateException) when (
                dbUpdateException.InnerException is SqlException sqlException &&
                sqlException.IsAlreadyProcessedException())
            {
                //TODO Logs
                Console.WriteLine("UNIQUE VIOLATION");
            }
            catch (SqlException sqlException) when (sqlException.IsAlreadyProcessedException())
            {
                //TODO Logs
                Console.WriteLine("UNIQUE VIOLATION");
            }
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("processed-event-filter");
        }

        private void MessageAlreadyProcessed(Guid messageId, string consumer)
        {
            //TODO Logs
            Console.WriteLine($"MESSAGE ALREADY PROCESSED {messageId} BY {consumer}");
        }
    }
}