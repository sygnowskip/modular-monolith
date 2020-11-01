using ModularMonolith.Language.Subjects;

namespace ModularMonolith.ReadModels
{
    public class Subject
    {
        internal Subject(SubjectId id, string name)
        {
            Id = id;
            Name = name;
        }

        public SubjectId Id { get; private set; }
        public string Name { get; private set; }
    }
}