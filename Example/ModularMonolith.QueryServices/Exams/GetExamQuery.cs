using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Contracts.Exams;
using ModularMonolith.Errors;
using ModularMonolith.QueryServices.Exams.Extensions;
using ModularMonolith.ReadModels;
using ModularMonolith.ReadModels.Planning;

namespace ModularMonolith.QueryServices.Exams
{
    public class GetExamQuery : IRequest<Result<ExamDto>>
    {
        private GetExamQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }

        public static Result<GetExamQuery> Create(long examId)
        {
            return Result.Ok(new GetExamQuery(examId));
        }
    }

    public class GetExamQueryHandler : IRequestHandler<GetExamQuery, Result<ExamDto>>
    {
        private readonly IMonolithQueryDbContext _monolithQueryDbContext;

        public GetExamQueryHandler(IMonolithQueryDbContext monolithQueryDbContext)
        {
            _monolithQueryDbContext = monolithQueryDbContext;
        }

        public async Task<Result<ExamDto>> Handle(GetExamQuery request, CancellationToken cancellationToken)
        {
            var exam = await _monolithQueryDbContext.Exams
                .Where(e => e.Id == request.Id)
                .Select(e => e.ToExamDto())
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);

            return exam == null
                ? Result.Fail<ExamDto>(DomainErrors.BuildNotFound(nameof(Exam), request.Id))
                : Result.Ok(exam);
        }
    }
}