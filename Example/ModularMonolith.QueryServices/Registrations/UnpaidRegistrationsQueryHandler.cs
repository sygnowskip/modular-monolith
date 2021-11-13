/*using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ModularMonolith.Registrations.Contracts.Queries;

namespace ModularMonolith.Registrations.Queries
{
    public class UnpaidRegistrationsQueryHandler : IRequestHandler<UnpaidRegistrations, IEnumerable<UnpaidRegistrationDto>>
    {
        private readonly MonolithQueryDbContext _queryDbContext;

        public UnpaidRegistrationsQueryHandler(MonolithQueryDbContext queryDbContext)
        {
            _queryDbContext = queryDbContext;
        }

        public async Task<IEnumerable<UnpaidRegistrationDto>> Handle(UnpaidRegistrations request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            /*return await _queryDbContext.Registrations
                .Where(r => r.Status == RegistrationStatus.AwaitingPayment || r.Status == RegistrationStatus.New)
                .Select(r => new UnpaidRegistrationDto(r.Id, r.Status))
                .ToListAsync(cancellationToken: cancellationToken);#1#
        }
    }
}*/