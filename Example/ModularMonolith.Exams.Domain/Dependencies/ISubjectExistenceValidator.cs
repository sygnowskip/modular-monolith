namespace ModularMonolith.Exams.Domain.Dependencies
{
    public interface ISubjectExistenceValidator
    {
        bool Exist(long subjectId);
    }
}