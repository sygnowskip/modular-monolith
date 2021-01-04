namespace Hexure.Deleting
{
    public interface IDeletableAggregate
    {
        bool IsDeleted { get; }
    }
}