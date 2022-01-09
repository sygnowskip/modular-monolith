using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Hexure.Events;
using Hexure.Events.Namespace;
using Hexure.Events.Serialization;
using ModularMonolith.Registrations.Events;
using ModularMonolith.Registrations.Language;
using Newtonsoft.Json;
using NUnit.Framework;

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
            var registrationId = new RegistrationId(1);
            var domainEvent = new RegistrationPaid(registrationId, new ExternalRegistrationId(), DateTime.UtcNow);

            var serialized = _eventSerializer.Serialize(domainEvent);

            serialized.IsSuccess.Should().BeTrue();
            serialized.Value.Namespace.Should().Be("Registrations");
            serialized.Value.Type.Should().Be(nameof(RegistrationPaid));
            serialized.Value.Payload.Should().Contain(registrationId.Value.ToString());
        }

        [Test]
        public void ShouldNotSerializeNullableEvents()
        {
            var serialized = _eventSerializer.Serialize(null);

            serialized.IsSuccess.Should().BeFalse();
            serialized.ViolatesOnly(EventSerializerErrors.UnableToSerializeNullEvent).Should().BeTrue();
        }

        [Test]
        public void ShouldNotSerializeEventsFromAssembliesWithoutDefinedNamespaces()
        {
            var domainEvent = new TestEvent(Guid.NewGuid(), DateTime.UtcNow);

            var serialized = _eventSerializer.Serialize(domainEvent);

            serialized.IsSuccess.Should().BeFalse();
            serialized.ViolatesOnly(EventSerializerErrors.UnableToSerializeEventFromAssemblyWithoutDefinedNamespace)
                .Should().BeTrue();
        }

        [Test]
        public void ShouldDeserializeEvent()
        {
            var registrationId = new RegistrationId(1);
            var publishedOn = DateTime.UtcNow;
            var domainEvent = new RegistrationPaid(registrationId, new ExternalRegistrationId(), publishedOn);

            var serialized = _eventSerializer.Serialize(domainEvent);

            var deserialized = _eventDeserializer.Deserialize(serialized.Value);

            deserialized.IsSuccess.Should().BeTrue();

            var deserializedEvent = deserialized.Value as IEvent;
            deserializedEvent.Should().NotBeNull();
            deserializedEvent.GetType().Should().Be<RegistrationPaid>();
            deserializedEvent.PublishedOn.Should().Be(publishedOn);
        }

        [Test]
        public void ShouldNotDeserializeEventFromUnspecifiedAssembly()
        {
            var domainEvent = new TestDomainEvent(DateTime.UtcNow);

            var serializedEvent = new SerializedEvent("Unknown", nameof(TestDomainEvent),
                JsonConvert.SerializeObject(domainEvent));

            var deserialized = _eventDeserializer.Deserialize(serializedEvent);

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