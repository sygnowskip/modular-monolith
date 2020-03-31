namespace Hexure
{
    public interface IAggregateRoot<out TIdentifier>
    {
        TIdentifier Id { get; }
    }
}