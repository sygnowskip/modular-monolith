using System.Linq;
using ModularMonolith.Language.Locations;
using ModularMonolith.ReadModels;

namespace ModularMonolith.Persistence.Read.Validators
{
    public class LocationExistenceValidator : ILocationExistenceValidator
    {
        private readonly IMonolithQueryDbContext _monolithQueryDbContext;

        public LocationExistenceValidator(IMonolithQueryDbContext monolithQueryDbContext)
        {
            _monolithQueryDbContext = monolithQueryDbContext;
        }

        public bool Exist(long locationId)
        {
            return _monolithQueryDbContext.Locations.Any(location => location.Id == new LocationId(locationId));
        }
    }
}