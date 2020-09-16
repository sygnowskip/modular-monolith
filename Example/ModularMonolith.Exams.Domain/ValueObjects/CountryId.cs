using System.Collections.Generic;
using Hexure.Results;
using Hexure.Results.Extensions;
using ModularMonolith.Exams.Domain.Dependencies;

namespace ModularMonolith.Exams.Domain.ValueObjects
{
    public class CountryId : ValueObject
    {
        private CountryId(long value)
        {
            Value = value;
        }

        public long Value { get; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static Result<CountryId> Create(long countryId, ICountryExistenceValidator countryExistenceValidator)
        {
            return Result.Create(() => countryId > 0, CountryIdErrors.CountryDoesNotExist.Build())
                .AndEnsure(() => countryExistenceValidator.Exist(countryId), CountryIdErrors.CountryDoesNotExist.Build())
                .OnSuccess(() => new CountryId(countryId));
        }
    }

    public class CountryIdErrors
    {
        public static readonly Error.ErrorType CountryDoesNotExist = new Error.ErrorType(nameof(CountryDoesNotExist), "Country for provided identifier does not exist");
    }
}