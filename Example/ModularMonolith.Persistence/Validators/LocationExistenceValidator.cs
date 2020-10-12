using System.Linq;
using ModularMonolith.Language.Locations;

namespace ModularMonolith.Persistence.Validators
{
    internal class LocationExistenceValidator : ILocationExistenceValidator
    {
        private readonly MonolithDbContext _monolithDbContext;

        public LocationExistenceValidator(MonolithDbContext monolithDbContext)
        {
            _monolithDbContext = monolithDbContext;
        }

        public bool Exist(long locationId)
        {
            return _monolithDbContext.Locations.Any(location => location.Id == new LocationId(locationId));
        }
    }
}