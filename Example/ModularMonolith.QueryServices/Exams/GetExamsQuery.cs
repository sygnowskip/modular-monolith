using System.Collections.Generic;
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
}