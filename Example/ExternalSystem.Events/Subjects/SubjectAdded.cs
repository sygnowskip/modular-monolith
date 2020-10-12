namespace ExternalSystem.Events.Subjects
{
    public class SubjectAdded
    {
        public SubjectAdded(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
    }
}