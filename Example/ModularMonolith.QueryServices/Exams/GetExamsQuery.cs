using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Contracts.Exams;
using ModularMonolith.QueryServices.Exams.Extensions;
using ModularMonolith.ReadModels;

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
        private readonly IMonolithQueryDbContext _monolithQueryDbContext;

        public GetExamsQueryHandler(IMonolithQueryDbContext monolithQueryDbContext)
        {
            _monolithQueryDbContext = monolithQueryDbContext;
        }

        public async Task<IEnumerable<ExamDto>> Handle(GetExamsQuery request, CancellationToken cancellationToken)
        {
            //TODO: Pagination, filters?
            return await _monolithQueryDbContext.Exams
                .Select(e => e.ToExamDto())
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}