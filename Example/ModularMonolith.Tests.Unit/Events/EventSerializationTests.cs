using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Hexure.Events;
using Hexure.Events.Namespace;
using Hexure.Events.Serialization;
using ModularMonolith.Payments.Contracts.Events;
using ModularMonolith.Payments.Language;
using ModularMonolith.Registrations.Contracts.Events;
using ModularMonolith.Registrations.Language;
using NUnit.Framework;

[assembly: EventNamespace("Tests")]

namespace ModularMonolith.Tests.Unit.Events
{
    public class TestDomainEvent : IEvent
    {
        public TestDomainEvent(DateTime publishedOn)
        {
            PublishedOn = publishedOn;
        }

        public DateTime PublishedOn { get; }
    }

    [TestFixture]
    public class EventSerializationTests
    {
        private readonly IEventSerializer _eventSerializer = new EventSerializer(new EventNamespaceReader());
        private readonly IEventDeserializer _eventDeserializer = new EventDeserializer(new EventTypeProvider(new Dictionary<string, Assembly>()
        {
            {"Registrations", typeof(RegistrationPaid).Assembly}
        }));

        [Test]
        public void ShouldSerializeDomainEvent()
        {
            var registrationId = new RegistrationId(Guid.NewGuid());
            var domainEvent = new RegistrationPaid(registrationId, DateTime.UtcNow);

            var serialized = _eventSerializer.Serialize(domainEvent);

            serialized.IsSuccess.Should().BeTrue();
            serialized.Value.Namespace.Should().Be("Registrations");
            serialized.Value.Type.Should().Be(nameof(RegistrationPaid));
            serialized.Value.Payload.Should().Contain(registrationId.Value.ToString());
        }

        [Test]
        public void ShouldNotSerializeNullableEvents()
        {
            var serialized = _eventSerializer.Serialize<RegistrationPaid>(null);

            serialized.IsSuccess.Should().BeFalse();
            serialized.ViolatesOnly(EventSerializerErrors.UnableToSerializeNullEvent).Should().BeTrue();
        }

        [Test]
        public void ShouldNotSerializeEventsFromAssembliesWithoutDefinedNamespaces()
        {
            var domainEvent = new PaymentCompleted(new PaymentId(Guid.NewGuid()), Guid.NewGuid(), DateTime.UtcNow);

            var serialized = _eventSerializer.Serialize(domainEvent);

            serialized.IsSuccess.Should().BeFalse();
            serialized.ViolatesOnly(EventSerializerErrors.UnableToSerializeEventFromAssemblyWithoutDefinedNamespace)
                .Should().BeTrue();
        }

        [Test]
        public void ShouldDeserializeEvent()
        {
            var registrationId = new RegistrationId(Guid.NewGuid());
            var publishedOn = DateTime.UtcNow;
            var domainEvent = new RegistrationPaid(registrationId, publishedOn);

            var serialized = _eventSerializer.Serialize(domainEvent);

            var deserialized = _eventDeserializer.Deserialize(serialized.Value);

            deserialized.IsSuccess.Should().BeTrue();
            deserialized.Value.GetType().Should().Be<RegistrationPaid>();
            deserialized.Value.PublishedOn.Should().Be(publishedOn);
        }

        [Test]
        public void ShouldNotDeserializeEventFromUnspecifiedAssembly()
        {
            var domainEvent = new TestDomainEvent(DateTime.UtcNow);

            var serialized = _eventSerializer.Serialize(domainEvent);

            var deserialized = _eventDeserializer.Deserialize(serialized.Value);

            deserialized.IsSuccess.Should().BeFalse();
            deserialized.ViolatesOnly(EventDeserializerErrors.UnableToFindTypeForEvent).Should().BeTrue();
        }
        
        [Test]
        public void ShouldNotDeserializeNullEvent()
        {
            var deserialized = _eventDeserializer.Deserialize(null);

            deserialized.IsSuccess.Should().BeFalse();
            deserialized.ViolatesOnly(EventDeserializerErrors.UnableToDeserializeNullEvent).Should().BeTrue();
        }
    }
}