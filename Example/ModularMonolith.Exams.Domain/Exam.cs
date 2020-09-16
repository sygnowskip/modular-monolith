using Hexure;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.Exams.Domain
{
    public class Exam : IAggregateRoot<ExamId>
    {
        public ExamId Id { get; }
    }
}