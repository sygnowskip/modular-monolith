using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Hexure.EntityFrameworkCore;
using Hexure.MediatR;
using Hexure.MediatR.Behaviors;
using Hexure.Results;
using MediatR;
using Moq;
using NUnit.Framework;

namespace Hexure.Tests.Unit.MediatR
{
    [TestFixture]
    public class TransactionalCommandsBehaviorTests
    {
        private Mock<ITransactionProvider> _transactionProviderMock;
        private ITransactionalBehaviorValidator _transactionalBehaviorValidator;

        [SetUp]
        public void SetUp()
        {
            _transactionProviderMock = new Mock<ITransactionProvider>();
            _transactionalBehaviorValidator = new TransactionalBehaviorValidator();
        }

        [Test]
        public async Task ShouldIgnoreMediatorRequests()
        {
            var behavior = new TransactionalCommandsBehavior<WrongCommand, int>(_transactionProviderMock.Object, _transactionalBehaviorValidator);
            await behavior.Handle(new WrongCommand(), CancellationToken.None, () => Task.FromResult(2));

            _transactionProviderMock.Verify(provider => provider.BeginTransactionAsync(), Times.Never);
        }

        [Test]
        public async Task ShouldOpenTransactionsBeforeCommandExecution()
        {
            var behavior = new TransactionalCommandsBehavior<Command, Result<int>>(_transactionProviderMock.Object, _transactionalBehaviorValidator);
            await behavior.Handle(new Command(), CancellationToken.None, () => Task.FromResult(Result.Ok(2)));

            _transactionProviderMock.Verify(provider => provider.BeginTransactionAsync(), Times.Once);
        }

        [Test]
        public async Task ShouldCommitTransactionAfterSuccessfulCommandExecution()
        {
            var behavior = new TransactionalCommandsBehavior<Command, Result<int>>(_transactionProviderMock.Object, _transactionalBehaviorValidator);
            await behavior.Handle(new Command(), CancellationToken.None, () => Task.FromResult(Result.Ok(2)));

            _transactionProviderMock.Verify(provider => provider.CommitTransactionAsync(), Times.Once);
        }

        [Test]
        public async Task ShouldRollbackTransactionAfterFailureCommandExecution()
        {
            var invalidExecutionError = new Error.ErrorType("INVALID_EXECUTION", "Invalid execution").Build();
            var behavior = new TransactionalCommandsBehavior<Command, Result<int>>(_transactionProviderMock.Object, _transactionalBehaviorValidator);
            await behavior.Handle(new Command(), CancellationToken.None, () => Task.FromResult(Result.Fail<int>(invalidExecutionError)));

            _transactionProviderMock.Verify(provider => provider.RollbackTransactionAsync(), Times.Once);
        }

        [Test]
        public void ShouldRollbackTransactionAfterExceptionInCommandExecution()
        {
            var behavior = new TransactionalCommandsBehavior<Command, Result<int>>(_transactionProviderMock.Object, _transactionalBehaviorValidator);
            Func<Task> handleAction = async () => await behavior.Handle(new Command(), CancellationToken.None, () => throw new Exception("Inavlid execution"));

            handleAction.Should().Throw<Exception>();
            _transactionProviderMock.Verify(provider => provider.RollbackTransactionAsync(), Times.Once);
        }

        #region Commands definition

        private class WrongCommand : IRequest
        {
            
        }

        private class Command : ICommandRequest
        {
            
        }

        #endregion
    }
}