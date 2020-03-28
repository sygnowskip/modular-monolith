using System;
using Hexure.Results.Extensions;
using Newtonsoft.Json;

namespace Hexure.Results.Deserialization
{
    public class MaybeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(Maybe<>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return objectType.GetPropertyValue("None", null);
            }

            var valueType = objectType.GetGenericArguments()[0];
            var value = serializer.Deserialize(reader, valueType);

            return objectType.GetMethod("From").Invoke(null, new[] { value });
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = value.GetType();
            var hasValue = (bool)type.GetPropertyValue("HasValue", value);

            if (hasValue)
            {
                var wrappedValue = type.GetPropertyValue("Value", value);
                serializer.Serialize(writer, wrappedValue);
            }
            else
            {
                serializer.Serialize(writer, null);
            }
        }
    }
}