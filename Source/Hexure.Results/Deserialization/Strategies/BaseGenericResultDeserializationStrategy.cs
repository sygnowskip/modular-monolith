using System;
using System.Linq;
using System.Reflection;
using Hexure.Results.Extensions;
using Newtonsoft.Json;

namespace Hexure.Results.Deserialization.Strategies
{
    internal abstract class BaseGenericResultDeserializationStrategy
    {
        protected abstract Type GetDeserializableResultType();
        protected abstract Type GetContainingResultsFactoryType();

        protected object BaseDeserialize(JsonReader reader, Type objectType, JsonSerializer serializer)
        {
            var valueType = objectType.GenericTypeArguments[0];

            var deserializableResultClosedType = GetDeserializableResultType().MakeGenericType(valueType);

            var deserializableResult = (IResult)serializer.Deserialize(reader, deserializableResultClosedType);

            if (deserializableResult.IsSuccess)
            {
                return CreateOkResult(deserializableResultClosedType, deserializableResult, valueType);
            }

            return CreateFailResult(deserializableResultClosedType, deserializableResult, valueType);
        }

        private object CreateFailResult(Type deserializableResultClosedType, IResult deserializableResult, Type genericResultArgument)
        {
            var error = deserializableResultClosedType.GetPropertyValue("Error", deserializableResult);

            var genericResultFailStaticFactory = GetFailResultFactoryMethod();

            return genericResultFailStaticFactory.MakeGenericMethod(genericResultArgument).Invoke(null, new[] { error });
        }

        private object CreateOkResult(Type deserializableResultClosedType, IResult deserializableResult, Type genericResultArgument)
        {
            var value = deserializableResultClosedType.GetPropertyValue("Value", deserializableResult);

            var genericResultOkStaticFactory = GetOkResultFactoryMethod();

            return genericResultOkStaticFactory.MakeGenericMethod(genericResultArgument).Invoke(null, new[] { value });
        }

        private MethodInfo GetOkResultFactoryMethod()
        {
            return GetContainingResultsFactoryType()
                .GetMethods()
                .Single(m => m.GetCustomAttributes(typeof(ResultOkFactoryMethodAttribute), false).Any());
        }

        private MethodInfo GetFailResultFactoryMethod()
        {
            return GetContainingResultsFactoryType()
                .GetMethods()
                .Single(m => m.GetCustomAttributes(typeof(ResultFailFactoryMethodAttribute), false).Any());
        }
    }

    /// <summary>
    /// Used only to mark Result.Ok<TValue>() factory method
    /// </summary>
    internal class ResultOkFactoryMethodAttribute : Attribute
    {
    }

    /// <summary>
    /// Used only to mark Result.Fail<TValue>() factory method
    /// </summary>
    internal class ResultFailFactoryMethodAttribute : Attribute
    {
    }
}
