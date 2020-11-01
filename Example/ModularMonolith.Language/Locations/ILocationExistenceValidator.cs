namespace ModularMonolith.Language.Locations
{
    public interface ILocationExistenceValidator
    {
        bool Exist(long locationId);
    }
}