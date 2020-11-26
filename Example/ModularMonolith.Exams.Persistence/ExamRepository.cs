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
        private readonly IExamDbContext _examDbContext;

        public ExamRepository(IExamDbContext examDbContext)
        {
            _examDbContext = examDbContext;
        }

        public async Task<Result<Exam>> SaveAsync(Exam aggregate)
        {
            await _examDbContext.Exams.AddAsync(aggregate);
            await _examDbContext.SaveChangesAsync();
            return Result.Ok(aggregate);
        }

        public async Task<Result<Exam>> GetAsync(ExamId identifier)
        {
            return Maybe<Exam>.From(await _examDbContext.Exams.SingleOrDefaultAsync(exam => exam.Id == identifier))
                .ToResult(DomainErrors.BuildAggregateNotFound(nameof(Exam), identifier.Value));
        }

        public Task<Result> Delete(Exam aggregate)
        {
            _examDbContext.Exams.Remove(aggregate);

            return Task.FromResult(Result.Ok());
        }
    }
}