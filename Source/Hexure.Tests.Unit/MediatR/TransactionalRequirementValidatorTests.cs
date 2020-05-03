using FluentAssertions;
using Hexure.MediatR;
using MediatR;
using NUnit.Framework;

namespace Hexure.Tests.Unit.MediatR
{
    [TestFixture]
    public class TransactionalRequirementValidatorTests
    {
        private readonly ITransactionalBehaviorValidator _transactionalBehaviorValidator = new TransactionalBehaviorValidator();

        [Test]
        public void ShouldReturnTrueForCorrectCommands()
        {
            var commandResult = _transactionalBehaviorValidator.IsCommand<Command>();
            var genericCommandResult = _transactionalBehaviorValidator.IsCommand<GenericCommand>();

            commandResult.Should().BeTrue();
            genericCommandResult.Should().BeTrue();
        }

        [Test]
        public void ShouldReturnFalseForOtherMediatorRequests()
        {
            var wrongCommandResult = _transactionalBehaviorValidator.IsCommand<WrongCommand>();

            wrongCommandResult.Should().BeFalse();
        }

        #region Commands definition
        private class Command : ICommandRequest
        {

        }

        private class GenericCommand : ICommandRequest<string>
        {

        }

        private class WrongCommand : IRequest
        {

        }
        #endregion
    }
}