using System.Linq;
using ModularMonolith.Exams.Language;
using ModularMonolith.Exams.Language.Validators;

namespace ModularMonolith.Exams.Persistence.Validators
{
    internal class ExamExistenceValidator : IExamExistenceValidator
    {
        private readonly IExamsDbContext _dbContext;

        public ExamExistenceValidator(IExamsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Exist(long examId)
        {
            return _dbContext.Exams.Any(e => e.Id == new ExamId(examId));
        }
    }
}