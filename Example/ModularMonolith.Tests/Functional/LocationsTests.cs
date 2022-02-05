using System;
using System.Linq;
using System.Threading.Tasks;
using ExternalSystem.Events.Locations;
using FluentAssertions;
using ModularMonolith.Tests.Common;
using NUnit.Framework;

namespace ModularMonolith.Tests.Functional
{
    [TestFixture]
    public class LocationsTests : BaseScenariosTests
    {
        [Test]
        public async Task ShouldGetListOfSubjectsAsync()
        {
            Scenarios.Locations.Given().HaveLocationsInDatabase();

            var locations = await Scenarios.Locations.When().GetLocationsAsync();

            locations.Should().NotBeEmpty();
        }

        [Test]
        public async Task ShouldSynchroniseLocationAfterAdd()
        {
            var locations = await Scenarios.Locations.Given().GetLocationsAsync();

            await Scenarios.Messages.When().MessageIsPublishedAsync(new LocationAdded(6, "Moscow", DateTime.UtcNow));

            await Scenarios.Locations.Then().LocationCountShouldEqualToAsync(locations.Count() + 1);
        }

        [Test]
        public async Task ShouldSynchroniseLocationAfterDelete()
        {
            var locations = await Scenarios.Locations.Given().GetLocationsAsync();

            await Scenarios.Messages.When().MessageIsPublishedAsync(new LocationDeleted(5, DateTime.UtcNow));

            await Scenarios.Locations.Then().LocationCountShouldEqualToAsync(locations.Count() - 1);
        }
    }
}