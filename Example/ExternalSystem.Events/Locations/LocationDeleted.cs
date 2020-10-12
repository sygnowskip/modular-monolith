namespace ExternalSystem.Events.Locations
{
    public class LocationDeleted
    {
        public LocationDeleted(long id)
        {
            Id = id;
        }

        public long Id { get; private set; }
    }
}