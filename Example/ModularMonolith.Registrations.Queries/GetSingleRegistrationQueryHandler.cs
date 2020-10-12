/*using System.Threading;
using System.Threading.Tasks;
using Hexure.Results;
using MediatR;
using ModularMonolith.Registrations.Contracts.Queries;

namespace ModularMonolith.Registrations.Queries
{
    public class GetSingleRegistrationQueryHandler : IRequestHandler<GetSingleRegistration, Result<GetSingleRegistrationDto>>
    {
        private readonly MonolithQueryDbContext _queryDbContext;

        public GetSingleRegistrationQueryHandler(MonolithQueryDbContext queryDbContext)
        {
            _queryDbContext = queryDbContext;
        }

        public async Task<Result<GetSingleRegistrationDto>> Handle(GetSingleRegistration request, CancellationToken cancellationToken)
        {
            return Result.Ok<GetSingleRegistrationDto>(null);

            /*var registration = await _queryDbContext.Registrations
                .Where(r => r.Id == request.Id)
                .Select(r => new GetSingleRegistrationDto(r.Id, r.Status, r.CandidateFirstName, r.CandidateLastName, r.CandidateDateOfBirth))
                .SingleOrDefaultAsync(cancellationToken);

            return Maybe<GetSingleRegistrationDto>.From(registration)
                .ToResult(GetSingleRegistrationErrors.UnableToFindRegistration.Build());#1#
        }
    }
}*/