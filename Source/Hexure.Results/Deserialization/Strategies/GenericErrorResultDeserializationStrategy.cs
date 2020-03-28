using System;
using Newtonsoft.Json;

namespace Hexure.Results.Deserialization.Strategies
{
    internal class GenericErrorResultDeserializationStrategy : BaseGenericResultDeserializationStrategy, IResultDeserializationStrategy
    {
        public static bool CanHandle(Type objectType) => objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Result<>);

        public static readonly IResultDeserializationStrategy Deserializer = new GenericErrorResultDeserializationStrategy();

        private GenericErrorResultDeserializationStrategy() { }

        protected override Type GetDeserializableResultType() => typeof(DeserializableErrorResult<>);

        protected override Type GetContainingResultsFactoryType() => typeof(Result);

        public object Deserialize(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            if (!CanHandle(objectType))
            {
                throw new ArgumentException($"{nameof(GenericErrorResultDeserializationStrategy)} can handle only ErrorResults.Result<TValue>.");
            }

            return BaseDeserialize(reader, objectType, serializer);
        }

        private class DeserializableErrorResult<TValue> : IResult
        {
            public bool IsFailure { get; set; }
            public bool IsSuccess { get; set; }
            public Error[] Error { get; set; }
            public TValue Value { get; set; }
        }
    }
}