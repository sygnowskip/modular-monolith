using System.Collections.Generic;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore.Events.Entites;
using Hexure.Events;
using Hexure.Events.Publishing;
using Hexure.Events.Serialization;
using Hexure.Results.Extensions;

namespace Hexure.EntityFrameworkCore.Events.Publishing
{
    public class DatabaseEventPublisher : IEventPublisher
    {
        private readonly IEventSerializer _eventSerializer;
        private readonly ISerializedEventDbContext _dbContext;

        public DatabaseEventPublisher(IEventSerializer eventSerializer, ISerializedEventDbContext dbContext)
        {
            _eventSerializer = eventSerializer;
            _dbContext = dbContext;
        }

        public async Task Publish(IEnumerable<IEvent> events)
        {
            await _dbContext.SerializedEvents.AddRangeAsync(CreateEntities(events));
        }

        private IEnumerable<SerializedEventEntity> CreateEntities(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                yield return _eventSerializer.Serialize(@event)
                    .OnSuccess(SerializedEventEntity.Create)
                    .Value;
            }
        }
    }
}