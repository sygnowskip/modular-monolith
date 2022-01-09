using System;
using System.Linq;
using System.Threading.Tasks;
using ExternalSystem.Events.Subjects;
using FluentAssertions;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.Functional
{
    [TestFixture]
    public class SubjectsTests : BaseScenariosTests
    {
        [Test]
        public async Task ShouldGetListOfSubjectsAsync()
        {
            Scenarios.Subjects.Given().HaveSubjectsInDatabase();

            var subjects = await Scenarios.Subjects.When().GetSubjectsAsync();

            subjects.Should().NotBeEmpty();
        }

        [Test]
        public async Task ShouldSynchroniseSubjectsAfterAdd()
        {
            var subjects = await Scenarios.Subjects.Given().GetSubjectsAsync();

            await Scenarios.Messages.When().MessageIsPublishedAsync(new SubjectAdded(6, "Economy", DateTime.UtcNow));

            await Scenarios.Subjects.Then().SubjectsCountShouldBeEqualToAsync(subjects.Count() + 1);
        }

        [Test]
        public async Task ShouldSynchroniseSubjectsAfterDelete()
        {
            var subjects = await Scenarios.Subjects.Given().GetSubjectsAsync();

            await Scenarios.Messages.When().MessageIsPublishedAsync(new SubjectDeleted(5, DateTime.UtcNow));

            await Scenarios.Subjects.Then().SubjectsCountShouldBeEqualToAsync(subjects.Count() - 1);
        }
    }
}