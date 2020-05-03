using System;
using System.Threading;
using System.Threading.Tasks;
using Hexure.EntityFrameworkCore;
using Hexure.Results;
using MediatR;

namespace Hexure.MediatR.Behaviors
{
    public class TransactionalCommandsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ITransactionProvider _transactionProvider;
        private readonly ITransactionalBehaviorValidator _transactionalBehaviorValidator;

        public TransactionalCommandsBehavior(ITransactionProvider transactionProvider, ITransactionalBehaviorValidator transactionalBehaviorValidator)
        {
            _transactionProvider = transactionProvider;
            _transactionalBehaviorValidator = transactionalBehaviorValidator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_transactionalBehaviorValidator.IsCommand<TRequest>())
            {
                await _transactionProvider.BeginTransactionAsync();
                try
                {
                    var response = await next();
                    if (response is IResult result)
                    {
                        if (result.IsSuccess)
                        {
                            await _transactionProvider.CommitTransactionAsync();
                        }
                        else
                        {
                            await _transactionProvider.RollbackTransactionAsync();
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("The transaction status for the command could not be determined");
                    }

                    return response;
                }
                catch
                {
                    await _transactionProvider.RollbackTransactionAsync();
                    //TODO: Logging
                    throw;
                }
            }

            return await next();
        }
    }
}