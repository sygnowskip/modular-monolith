using FluentAssertions;
using Hexure.Events.Namespace;
using ModularMonolith.Payments.Contracts.Events;
using ModularMonolith.Registrations.Contracts.Events;
using NUnit.Framework;

namespace ModularMonolith.Tests.Unit
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
            var eventsNamespace = _eventNamespaceReader.GetFromAssemblyOfType<PaymentCompleted>();

            eventsNamespace.HasValue.Should().BeFalse();
        }
    }
}