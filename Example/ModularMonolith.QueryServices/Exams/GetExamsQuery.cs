using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hexure.Results;
using MediatR;
using ModularMonolith.Contracts.Exams;

namespace ModularMonolith.QueryServices.Exams
{
    public class GetExamsQuery : IRequest<IEnumerable<ExamDto>>
    {
        public static Result<GetExamsQuery> Create()
        {
            return Result.Ok(new GetExamsQuery());
        }
    }

    public class GetExamsQueryHandler : IRequestHandler<GetExamsQuery, IEnumerable<ExamDto>>
    {
        private readonly IQueryBuilder _queryBuilder;
        private readonly IDbConnection _dbConnection;

        public GetExamsQueryHandler(IQueryBuilder queryBuilder, IDbConnection dbConnection)
        {
            _queryBuilder = queryBuilder;
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<ExamDto>> Handle(GetExamsQuery request, CancellationToken cancellationToken)
        {
            //TODO: Pagination, filters?
            return await _dbConnection.QueryAsync<ExamDto>(_queryBuilder.MultipleExamsQuery());
        }
    }
}