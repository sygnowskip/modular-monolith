using System;
using Newtonsoft.Json;

namespace Hexure.Results.Deserialization
{
    internal interface IResultDeserializationStrategy
    {
        object Deserialize(JsonReader reader, Type objectType, JsonSerializer serializer);
    }
}
