namespace ModularMonolith.Exams.Domain.Dependencies
{
    public interface ICountryExistenceValidator
    {
        bool Exist(long countryId);
    }
}