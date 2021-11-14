using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Errors
{
    [TestFixture]
    public class MaxLengthTests
    {
        [TestCase(null, 10, true, "Nullable string is shorter than 10 characters")]
        [TestCase("", 10, true, "Empty string is shorter than 10 characters")]
        [TestCase("John Smith", 10, true, "Provided string is shorter than 10 characters")]
        [TestCase("John Andrew Smith", 10, false, "Provided string is longer than 10 characters")]
        public void ShouldReturnExpectedResults(string value, int maxLength, bool expected, string because)
        {
            var property = "FirstName";

            var result = Language.Errors.MaxLength.Check(value, maxLength, property);

            result.IsSuccess.Should().Be(expected, because);
            if (!expected)
            {
                result.Error.Any(e => e.PropertyName == property).Should().BeTrue();
                result.Error.Any(e => e.Message.Contains(property) && e.Message.Contains($"{maxLength}")).Should().BeTrue();
            }
        }
    }
}