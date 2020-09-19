namespace ModularMonolith.Exams.Domain.Dependencies
{
    public interface ILocationExistenceValidator
    {
        bool Exist(long locationId);
    }
}