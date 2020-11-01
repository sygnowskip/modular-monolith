using System.Threading.Tasks;
using Hexure.Results;
using Microsoft.EntityFrameworkCore;
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
            return Result.Ok(aggregate);
        }

        public async Task<Maybe<Exam>> GetAsync(ExamId identifier)
        {
            return await _examDbContext.Exams.SingleOrDefaultAsync(exam => exam.Id == identifier);
        }

        public Task<Result> Delete(Exam aggregate)
        {
            _examDbContext.Exams.Remove(aggregate);

            return Task.FromResult(Result.Ok());
        }
    }
}