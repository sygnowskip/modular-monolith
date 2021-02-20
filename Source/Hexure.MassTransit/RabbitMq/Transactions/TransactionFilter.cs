using System;
using System.Threading.Tasks;
using GreenPipes;
using Hexure.EntityFrameworkCore;
using MassTransit;

namespace Hexure.MassTransit.RabbitMq.Transactions
{
    public class TransactionFilter<TMessage> : IFilter<ConsumeContext<TMessage>>
        where TMessage : class
    {
        private readonly ITransactionProvider _transactionProvider;

        public TransactionFilter(ITransactionProvider transactionProvider)
        {
            _transactionProvider = transactionProvider;
        }

        public async Task Send(ConsumeContext<TMessage> context, IPipe<ConsumeContext<TMessage>> next)
        {
            try
            {
                await _transactionProvider.BeginTransactionAsync();
                await next.Send(context);
                await _transactionProvider.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                //TODO: Logging
                Console.WriteLine(ex.ToString());
                await _transactionProvider.RollbackTransactionAsync();
                throw;
            }
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("transaction-filter");
        }
    }
}