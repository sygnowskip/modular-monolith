using System.Collections.Generic;
using Hexure.Identifiers.Numeric;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Errors;

namespace ModularMonolith.Language.Locations
{
    public class LocationId : Identifier
    {
        internal LocationId(long value) : base(value)
        {
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<LocationId> Create(long locationId, ILocationExistenceValidator locationExistenceValidator)
        {
            return Result.Create(locationId > 0, DomainErrors.BuildInvalidIdentifier(locationId))
                .AndEnsure(() => locationExistenceValidator.Exist(locationId),
                    DomainErrors.BuildNotFound("Location", locationId))
                .OnSuccess(() => new LocationId(locationId));
        }
    }
}