using FluentAssertions;
using Hexure.Results.Extensions;
using NUnit.Framework;

namespace Hexure.Tests.Unit.Results
{
    [TestFixture]
    public class ToCamelCaseTests
    {
        [TestCase("Value","value")]
        [TestCase("ComplexValue","complexValue")]
        [TestCase("SuperComplexValue","superComplexValue")]
        public void ShouldReturnCamelCaseString(string property, string expectedCamelCaseProperty)
        {
            var camelCaseProperty = property.ToCamelCase();
            camelCaseProperty.Should().Be(expectedCamelCaseProperty);
        }
        
    }
}