using System;
using Hexure.Time;

namespace Hexure.EntityFrameworkCore.Inbox.Entities
{
    public class ProcessedEventEntity
    {
        private ProcessedEventEntity(Guid messageId, string consumer, DateTime processedOn)
        {
            MessageId = messageId;
            Consumer = consumer;
            ProcessedOn = processedOn;
        }

        public long Id { get; private set; }
        public Guid MessageId { get; private set; }
        public string Consumer { get; private set; }
        public DateTime ProcessedOn { get; private set; }

        public static ProcessedEventEntity Create(Guid messageId, string consumer,
            ISystemTimeProvider systemTimeProvider)
        {
            return new ProcessedEventEntity(messageId, consumer, systemTimeProvider.UtcNow);
        }
    }
}