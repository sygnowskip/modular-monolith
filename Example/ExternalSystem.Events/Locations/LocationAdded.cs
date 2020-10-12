namespace ExternalSystem.Events.Locations
{
    public class LocationAdded
    {
        public LocationAdded(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
    }
}