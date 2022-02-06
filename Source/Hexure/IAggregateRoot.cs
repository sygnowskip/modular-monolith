using System;
using Hexure.Events;
using Hexure.Results;
using Hexure.Time;
using Stateless;

namespace Hexure
{
    public interface IAggregateRoot<out TIdentifier>
    {
        TIdentifier Id { get; }
    }

    public abstract class AggregateRoot<TIdentifier, TStates, TActions> : Entity, IAggregateRoot<TIdentifier>
        where TActions : Enum
        where TStates : Enum
    {
        protected readonly ISystemTimeProvider SystemTimeProvider;
        protected readonly StateMachine<TStates, TActions> StateMachine;

        public TIdentifier Id { get; protected set; }
        public TStates Status { get; protected set; }
        public DateTime DomainTimestamp { get; protected set; }

        protected AggregateRoot(ISystemTimeProvider systemTimeProvider)
        {
            SystemTimeProvider = systemTimeProvider;
            StateMachine = new StateMachine<TStates, TActions>(() => Status, status => Status = status);
            ConfigureStates();
        }

        protected Result CheckIfPossible<TError>(TActions action, TError error)
            where TError : Error
        {
            if (!StateMachine.CanFire(action))
            {
                return Result.Fail(error);
            }

            DomainTimestamp = SystemTimeProvider.UtcNow;
            StateMachine.Fire(action);

            return Result.Ok();
        }

        protected abstract void ConfigureStates();
    }
}