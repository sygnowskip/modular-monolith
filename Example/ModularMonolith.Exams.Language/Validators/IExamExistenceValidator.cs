namespace ModularMonolith.Exams.Language.Validators
{
    public interface IExamExistenceValidator
    {
        bool Exist(long examId);
    }
}