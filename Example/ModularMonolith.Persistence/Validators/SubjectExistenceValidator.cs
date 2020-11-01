using System.Linq;
using ModularMonolith.Language.Subjects;

namespace ModularMonolith.Persistence.Validators
{
    internal class SubjectExistenceValidator : ISubjectExistenceValidator
    {
        private readonly MonolithDbContext _monolithDbContext;

        public SubjectExistenceValidator(MonolithDbContext monolithDbContext)
        {
            _monolithDbContext = monolithDbContext;
        }

        public bool Exist(long subjectId)
        {
            return _monolithDbContext.Subjects.Any(subject => subject.Id == new SubjectId(subjectId));
        }
    }
}