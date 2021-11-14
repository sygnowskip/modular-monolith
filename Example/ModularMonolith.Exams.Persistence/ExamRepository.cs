using System.Threading.Tasks;
using Hexure.Results;
using Hexure.Results.Extensions;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Errors;
using ModularMonolith.Exams.Domain;
using ModularMonolith.Exams.Language;

namespace ModularMonolith.Exams.Persistence
{
    public class ExamRepository : IExamRepository
    {
        private readonly IExamsDbContext _examsDbContext;

        public ExamRepository(IExamsDbContext examsDbContext)
        {
            _examsDbContext = examsDbContext;
        }

        public async Task<Result<Exam>> SaveAsync(Exam aggregate)
        {
            await _examsDbContext.Exams.AddAsync(aggregate);
            await _examsDbContext.SaveChangesAsync();
            return Result.Ok(aggregate);
        }

        public async Task<Result<Exam>> GetAsync(ExamId identifier)
        {
            return Maybe<Exam>.From(await _examsDbContext.Exams.SingleOrDefaultAsync(exam => exam.Id == identifier))
                .ToResult(DomainErrors.BuildAggregateNotFound(nameof(Exam), identifier.Value));
        }

        public Task<Result> Delete(Exam aggregate)
        {
            _examsDbContext.Exams.Remove(aggregate);

            return Task.FromResult(Result.Ok());
        }
    }
}