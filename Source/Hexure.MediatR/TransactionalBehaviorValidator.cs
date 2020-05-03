using System;

namespace Hexure.MediatR
{
    public interface ITransactionalBehaviorValidator
    {
        bool IsCommand<TRequest>();
    }

    public class TransactionalBehaviorValidator : ITransactionalBehaviorValidator
    {
        public bool IsCommand<TRequest>()
        {
            return IsCommandRequest(typeof(TRequest)) ||
                   IsGenericCommandRequest(typeof(TRequest));
        }

        private bool IsCommandRequest(Type command) => typeof(ICommandRequest).IsAssignableFrom(command);
        private bool IsGenericCommandRequest(Type command)
        {
            foreach (var @interface in command.GetInterfaces())
            {
                if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(ICommandRequest<>))
                    return true;
            }

            return false;
        }
    }
}