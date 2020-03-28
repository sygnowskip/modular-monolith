using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hexure.Results.Deserialization.Strategies
{
    internal class ErrorResultDeserializationStrategy : IResultDeserializationStrategy
    {
        public static bool CanHandle(Type objectType) => objectType == typeof(Result);

        public static readonly IResultDeserializationStrategy Deserializer = new ErrorResultDeserializationStrategy();

        private ErrorResultDeserializationStrategy() { }

        public object Deserialize(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            if (!CanHandle(objectType))
            {
                throw new ArgumentException($"{nameof(ErrorResultDeserializationStrategy)} can handle only ErrorResults.Result.");
            }

            var deserializableResult = serializer.Deserialize<DeserializableErrorResult>(reader);

            if (deserializableResult.IsSuccess)
            {
                return Result.Ok();
            }

            return Result.Fail(deserializableResult.Error.ToArray());
        }

        private class DeserializableErrorResult : IResult
        {
            public bool IsFailure { get; set; }
            public bool IsSuccess { get; set; }
            public List<Error> Error { get; set; }
        }
    }
}