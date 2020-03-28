using System;
using Newtonsoft.Json;

namespace Hexure.Results.Deserialization
{
    internal sealed class ResultJsonConverter : JsonConverter
    {
        private static readonly IResultDeserializationStrategyFactory DeserializationStrategyFactory = new ResultDeserializationStrategyFactory();

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException($"{nameof(ResultJsonConverter)} should be used only for deserialization."); ;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var deserializationStrategy = DeserializationStrategyFactory.GetStrategy(objectType);

            return deserializationStrategy.Deserialize(reader, objectType, serializer);
        }

        public override bool CanConvert(Type objectType) => DeserializationStrategyFactory.CanHandle(objectType);
    }
}