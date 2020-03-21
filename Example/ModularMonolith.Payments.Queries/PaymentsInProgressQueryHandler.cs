using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ModularMonolith.Payments.Contracts.Queries;
using ModularMonolith.Payments.Language;
using ModularMonolith.Persistence.Read;

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
            return await _queryDbContext.Payments
                .Where(p => p.Status == PaymentStatus.New)
                .Select(p => new PaymentDto(p.Id, p.CorrelationId, p.Status))
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}