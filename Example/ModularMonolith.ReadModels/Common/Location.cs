using ModularMonolith.Language.Locations;

namespace ModularMonolith.ReadModels.Common
{
    public class Location
    {
        internal Location(LocationId id, string name)
        {
            Id = id;
            Name = name;
        }

        public LocationId Id { get; private set; }
        public string Name { get; private set; }
    }
}