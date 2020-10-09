using System.Linq;
using ModularMonolith.Language.Subjects;
using ModularMonolith.ReadModels;

namespace ModularMonolith.Persistence.Read.Validators
{
    public class SubjectExistenceValidator : ISubjectExistenceValidator
    {
        private readonly IMonolithQueryDbContext _monolithQueryDbContext;

        public SubjectExistenceValidator(IMonolithQueryDbContext monolithQueryDbContext)
        {
            _monolithQueryDbContext = monolithQueryDbContext;
        }

        public bool Exist(long subjectId)
        {
            return _monolithQueryDbContext.Subjects.Any(subject => subject.Id == new SubjectId(subjectId));
        }
    }
}