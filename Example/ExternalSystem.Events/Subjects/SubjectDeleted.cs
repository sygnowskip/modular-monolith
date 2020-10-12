namespace ExternalSystem.Events.Subjects
{
    public class SubjectDeleted
    {
        public SubjectDeleted(long id)
        {
            Id = id;
        }

        public long Id { get; private set; }
    }
}