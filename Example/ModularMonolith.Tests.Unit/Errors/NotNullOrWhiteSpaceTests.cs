using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Errors
{
    [TestFixture]
    public class NotNullOrWhiteSpaceTests
    {
        [TestCase(null)]
        [TestCase("")]
        public void ShouldReturnErrorWithPropertyName(string value)
        {
            var property = "FirstName";

            var result = Language.CommonErrors.NotNullOrWhiteSpace.Check(value, property);

            result.IsSuccess.Should().BeFalse();
            result.Error.Any(e => e.Message.Contains(property)).Should().BeTrue();
            result.Error.Any(e => e.PropertyName == property).Should().BeTrue();
        }

        [Test]
        public void ShouldReturnSuccessForNonEmptyValue()
        {
            var property = "FirstName";
            var value = "John";

            var result = Language.CommonErrors.NotNullOrWhiteSpace.Check(value, property);

            result.IsSuccess.Should().BeTrue();
        }
    }
}