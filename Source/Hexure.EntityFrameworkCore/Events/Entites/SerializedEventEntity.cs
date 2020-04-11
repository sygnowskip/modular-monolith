using System;
using Hexure.Events.Serialization;
using Hexure.Results;
using Hexure.Results.Extensions;

namespace Hexure.EntityFrameworkCore.Events.Entites
{
    public static class SerializedEventEntityErrors
    {
        public static readonly Error.ErrorType UnableToCreateEntityForEmptyEvent =
            new Error.ErrorType(nameof(UnableToCreateEntityForEmptyEvent), "Unable to create entity for empty event");
    }

    public class SerializedEventEntity
    {
        private SerializedEventEntity(SerializedEvent serializedEvent)
        {
            SerializedEvent = serializedEvent;
        }

        public long Id { get; private set; }
        public DateTime? ProcessedOn { get; private set; }
        public SerializedEvent SerializedEvent { get; private set; }

        public static Result<SerializedEventEntity> Create(SerializedEvent serializedEvent)
        {
            return Result.Create(serializedEvent != null,
                    SerializedEventEntityErrors.UnableToCreateEntityForEmptyEvent.Build())
                .OnSuccess(() => new SerializedEventEntity(serializedEvent));
        }
    }
}