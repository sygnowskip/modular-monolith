using Hexure.Results;
using Hexure.Results.Extensions;
using Stateless;

namespace ModularMonolith.Extensions
{
    public static class StateMachineExtensions
    {
        public static Result PerformIfPossible<TStatus, TAction>(this StateMachine<TStatus, TAction> stateMachine, TAction action, Error error)
        {
            return stateMachine
                .CanPerform(action, error)
                .OnSuccess(() => stateMachine.Fire(action));
        }
        
        private static Result CanPerform<TStatus, TAction>(this StateMachine<TStatus, TAction> stateMachine, TAction action, Error error)
        {
            if (!stateMachine.CanFire(action))
                return Result.Fail(error);

            return Result.Ok();
        }
    }
}