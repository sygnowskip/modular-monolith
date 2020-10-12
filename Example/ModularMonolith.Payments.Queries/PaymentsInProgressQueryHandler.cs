/*using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ModularMonolith.Payments.Contracts.Queries;

namespace ModularMonolith.Payments.Queries
{

    public class PaymentsInProgressQueryHandler : IRequestHandler<PaymentsInProgress, IEnumerable<PaymentDto>>
    {
        private readonly MonolithQueryDbContext _queryDbContext;

        public PaymentsInProgressQueryHandler(MonolithQueryDbContext queryDbContext)
        {
            _queryDbContext = queryDbContext;
        }

        public async Task<IEnumerable<PaymentDto>> Handle(PaymentsInProgress request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            /*return await _queryDbContext.Payments
                .Where(p => p.Status == PaymentStatus.New)
                .Select(p => new PaymentDto(p.Id, p.CorrelationId, p.Status))
                .ToListAsync(cancellationToken: cancellationToken);#1#
        }
    }
}*/