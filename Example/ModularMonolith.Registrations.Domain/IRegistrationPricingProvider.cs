using ModularMonolith.Language;
using ModularMonolith.Language.Locations;
using ModularMonolith.Language.Pricing;
using ModularMonolith.Language.Subjects;

namespace ModularMonolith.Registrations.Domain
{
    public interface IRegistrationPricingProvider
    {
        Price GetPrice(LocationId locationId, SubjectId subjectId, UtcDate examDate);
    }
}