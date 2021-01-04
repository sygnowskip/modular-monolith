using FluentAssertions;
using ModularMonolith.Errors;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit
{
    [TestFixture]
    public class DomainErrorsTests
    {
        [Test]
        public void ShouldReturnExpectedErrorForNotFound()
        {
            var entity = "Location";
            var identifier = 7;
            var notFoundError = DomainErrors.BuildNotFound(entity, identifier);

            notFoundError.Message.Should().Be($"{entity} for provided identifier {identifier} does not exist");
        }
        
        [Test]
        public void ShouldReturnExpectedErrorForInvalidIdentifier()
        {
            var identifier = 7;
            var notFoundError = DomainErrors.BuildInvalidIdentifier(identifier);

            notFoundError.Message.Should().Be($"Provided identifier {identifier} is invalid");
        }
    }
}