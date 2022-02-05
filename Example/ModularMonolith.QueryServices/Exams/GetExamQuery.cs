using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hexure.Results;
using MediatR;
using ModularMonolith.Contracts.Exams;
using ModularMonolith.Errors;

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
        private readonly IQueryBuilder _queryBuilder;
        private readonly IDbConnection _dbConnection;

        public GetExamQueryHandler(IQueryBuilder queryBuilder, IDbConnection dbConnection)
        {
            _queryBuilder = queryBuilder;
            _dbConnection = dbConnection;
        }

        public async Task<Result<ExamDto>> Handle(GetExamQuery request, CancellationToken cancellationToken)
        {
            //TODO: Dapper is not mapping dates as UTC, but as Unspecified
            var exam = await _dbConnection.QuerySingleAsync<ExamDto>(_queryBuilder.SingleExamQuery(),
                new {id = request.Id});

            return exam == null
                ? Result.Fail<ExamDto>(DomainErrors.BuildNotFound("Exam", request.Id))
                : Result.Ok(exam);
        }
    }
}