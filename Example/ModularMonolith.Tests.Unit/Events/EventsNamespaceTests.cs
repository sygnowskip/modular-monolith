﻿using FluentAssertions;
using Hexure.Events.Namespace;
using ModularMonolith.Registrations.Events;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit.Events
{
    [TestFixture]
    public class EventsNamespaceTests
    {
        private readonly IEventNamespaceReader _eventNamespaceReader = new EventNamespaceReader();

        [Test]
        public void ShouldReturnEventNamespaceValueBasedOnContractsType()
        {
            var eventsNamespace = _eventNamespaceReader.GetFromAssemblyOfType<RegistrationCreated>();

            eventsNamespace.HasValue.Should().BeTrue();
            eventsNamespace.Value.Name.Should().Be("Registrations");
        }

        [Test]
        public void ShouldReturnEmptyValueForAssemblyWithoutAttribute()
        {
            var eventsNamespace = _eventNamespaceReader.GetFromAssemblyOfType<TestEvent>();

            eventsNamespace.HasValue.Should().BeFalse();
        }

        [Test]
        public void ShouldReturnEventNamespaceValueBasedOnContractsRuntimeType()
        {
            var eventsNamespace = _eventNamespaceReader.GetFromAssemblyOfType(typeof(RegistrationCreated));

            eventsNamespace.HasValue.Should().BeTrue();
            eventsNamespace.Value.Name.Should().Be("Registrations");
        }

        [Test]
        public void ShouldReturnEmptyValueForAssemblyWithoutAttributeOnRuntimeType()
        {
            var eventsNamespace = _eventNamespaceReader.GetFromAssemblyOfType(typeof(TestEvent));

            eventsNamespace.HasValue.Should().BeFalse();
        }

        [Test]
        public void ShouldReturnEmptyValueForEventsThatDoesNotImplementIEventOnRuntimeType()
        {
            var eventsNamespace = _eventNamespaceReader.GetFromAssemblyOfType(typeof(int));

            eventsNamespace.HasValue.Should().BeFalse();
        }
    }
}