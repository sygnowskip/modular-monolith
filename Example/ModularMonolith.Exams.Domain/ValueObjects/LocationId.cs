﻿using System.Collections.Generic;
using Hexure.Identifiers.Numeric;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Exams.Domain.Dependencies;

namespace ModularMonolith.Exams.Domain.ValueObjects
{
    public class LocationId : Identifier
    {
        private LocationId(long value) : base(value)
        {
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<LocationId> Create(long locationId, ILocationExistenceValidator locationExistenceValidator)
        {
            return Result.Create(locationId > 0, LocationIdErrors.LocationDoesNotExist.Build())
                .AndEnsure(() => locationExistenceValidator.Exist(locationId), LocationIdErrors.LocationDoesNotExist.Build())
                .OnSuccess(() => new LocationId(locationId));
        }
    }

    public class LocationIdErrors
    {
        public static readonly Error.ErrorType LocationDoesNotExist = new Error.ErrorType(nameof(LocationDoesNotExist), "Location for provided identifier does not exist");
    }
}