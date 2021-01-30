using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore.Events;
using Hexure.EntityFrameworkCore.Events.Entites;
using Hexure.EntityFrameworkCore.Events.Repositories;
using Hexure.EntityFrameworkCore.SqlServer.Hints;
using Microsoft.EntityFrameworkCore;

namespace Hexure.EntityFrameworkCore.SqlServer.Events
{
    public class SerializedEventRepository : AbstractSerializedEventRepository
    {
        public SerializedEventRepository(ISerializedEventDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<IReadOnlyCollection<SerializedEventEntity>> GetUnpublishedEventsAsync(int batchSize)
        {
            return await DbContext
                .SerializedEvents
                .WithHint("XLOCK, ROWLOCK, READPAST")
                .OrderBy(entity => entity.Id)
                .Take(batchSize)
                .ToListAsync();
        }
    }
}