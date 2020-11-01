using Hexure;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.Exams.Domain
{
    public interface IExamRepository :  IAggregateRootRepository<Exam, ExamId>
    {
        
    }
}