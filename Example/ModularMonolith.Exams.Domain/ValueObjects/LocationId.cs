using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Exams.Domain.Dependencies;

namespace ModularMonolith.Exams.Domain.ValueObjects
{
    public class LocationId : ValueObject
    {
        private LocationId(CountryId countryId, long value)
        {
            CountryId = countryId;
            Value = value;
        }

        public CountryId CountryId { get; }
        public long Value { get; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return CountryId;
        }

        public static Result<LocationId> Create(long locationId, CountryId countryId, ILocationExistenceValidator locationExistenceValidator)
        {
            return Result.Create(locationId > 0, LocationIdErrors.LocationDoesNotExist.Build())
                .AndEnsure(() => locationExistenceValidator.Exist(locationId, countryId), LocationIdErrors.LocationDoesNotExist.Build())
                .OnSuccess(() => new LocationId(countryId, locationId));
        }
    }

    public class LocationIdErrors
    {
        public static readonly Error.ErrorType LocationDoesNotExist = new Error.ErrorType(nameof(LocationDoesNotExist), "Location for provided identifier does not exist");
    }
}