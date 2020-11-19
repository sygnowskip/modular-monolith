using System.Linq;
using ModularMonolith.Exams.Language;
using ModularMonolith.Exams.Language.Validators;

namespace ModularMonolith.Persistence.Validators
{
    internal class ExamExistenceValidator : IExamExistenceValidator
    {
        private readonly MonolithDbContext _monolithDbContext;

        public ExamExistenceValidator(MonolithDbContext monolithDbContext)
        {
            _monolithDbContext = monolithDbContext;
        }

        public bool Exist(long examId)
        {
            return _monolithDbContext.Exams.Any(e => e.Id == new ExamId(examId));
        }
    }
}