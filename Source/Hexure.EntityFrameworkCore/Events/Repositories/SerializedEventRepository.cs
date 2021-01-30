using System.Collections.Generic;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore.Events.Entites;

namespace Hexure.EntityFrameworkCore.Events.Repositories
{
    public interface ISerializedEventRepository
    {
        Task<IReadOnlyCollection<SerializedEventEntity>> GetUnpublishedEventsAsync(int batchSize);
        Task SaveChangesAsync();
        void RemoveRange(IEnumerable<SerializedEventEntity> processedEvents);
    }

    public abstract class AbstractSerializedEventRepository : ISerializedEventRepository
    {
        protected readonly ISerializedEventDbContext DbContext;

        protected AbstractSerializedEventRepository(ISerializedEventDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public abstract Task<IReadOnlyCollection<SerializedEventEntity>> GetUnpublishedEventsAsync(int batchSize);

        public Task SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        public void RemoveRange(IEnumerable<SerializedEventEntity> processedEvents)
        {
            DbContext.SerializedEvents.RemoveRange(processedEvents);
        }
    }
}