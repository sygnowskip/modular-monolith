using System.Threading.Tasks;
using ExternalSystem.Events.Locations;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Language.Locations;
using ModularMonolith.Persistence;

namespace ModularMonolith.ReadModels.EventHandlers.UpdateLocations
{
    internal class OnLocationDeleted : IConsumer<LocationDeleted>
    {
        private readonly MonolithDbContext _monolithDbContext;

        public OnLocationDeleted(MonolithDbContext monolithDbContext)
        {
            _monolithDbContext = monolithDbContext;
        }

        public async Task Consume(ConsumeContext<LocationDeleted> context)
        {
            var location = await _monolithDbContext.Locations.SingleOrDefaultAsync(l => l.Id == new LocationId(context.Message.Id));
            if (location == null)
                return;

            _monolithDbContext.Locations.Remove(location);
            await _monolithDbContext.SaveChangesAsync();
        }
    }
}