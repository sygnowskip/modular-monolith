using System.Threading.Tasks;
using ExternalSystem.Events.Locations;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Language.Locations;
using ModularMonolith.Persistence;
using ModularMonolith.ReadModels.Common;

namespace ModularMonolith.ReadModels.EventHandlers.UpdateLocations
{
    internal class OnLocationAdded : IConsumer<LocationAdded>
    {
        private readonly MonolithDbContext _monolithDbContext;

        public OnLocationAdded(MonolithDbContext monolithDbContext)
        {
            _monolithDbContext = monolithDbContext;
        }

        public async Task Consume(ConsumeContext<LocationAdded> context)
        {
            var locationExist = await _monolithDbContext.Locations.AnyAsync(l => l.Id == new LocationId(context.Message.Id));
            if (locationExist)
                return;

            var location = new Location(new LocationId(context.Message.Id),
                context.Message.Name);
            await _monolithDbContext.Locations.AddAsync(location);
        }
    }
}