using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using MediatR;
using ModularMonolith.Contracts.Registrations;

namespace ModularMonolith.QueryServices.Registrations
{
    public class GetRegistrationQuery : IRequest<Result<RegistrationDto>>
    {
        public GetRegistrationQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
        
        public static Result<GetRegistrationQuery> Create(long examId)
        {
            return Result.Ok(new GetRegistrationQuery(examId));
        }
    }
    
    public class GetRegistrationQueryHandler : IRequestHandler<GetRegistrationQuery, Result<RegistrationDto>>
    {
        public Task<Result<RegistrationDto>> Handle(GetRegistrationQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        private string BuildQuery()
        {
            return null;
        }
    }
}