using System;
using FluentAssertions;
using ModularMonolith.Language;
using NUnit.Framework;

namespace ModularMonolith.Exams.Tests.Unit
{
    [TestFixture]
    public class UtcDateTests
    {
        [TestCaseSource(nameof(_testCases))]
        public void ShouldReturnExpectedResult(DateTime dateTime, bool isSuccess)
        {
            var utcDateResult = UtcDate.Create(dateTime);

            utcDateResult.IsSuccess.Should().Be(isSuccess);
        }

        private static object[] _testCases =
        {
            new object[] {new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), true},
            new object[] {new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Local), false},
            new object[] {new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), false},
            new object[] {new DateTime(2020, 1, 1, 1, 0, 0, DateTimeKind.Utc), false},
            new object[] {new DateTime(2020, 1, 1, 1, 0, 0, DateTimeKind.Local), false},
            new object[] {new DateTime(2020, 1, 1, 1, 0, 0, DateTimeKind.Unspecified), false}
        };
    }
}