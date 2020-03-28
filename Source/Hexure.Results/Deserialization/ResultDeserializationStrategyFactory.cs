using System;
using Hexure.Results.Deserialization.Strategies;

namespace Hexure.Results.Deserialization
{
    internal interface IResultDeserializationStrategyFactory
    {
        IResultDeserializationStrategy GetStrategy(Type objectType);
        bool CanHandle(Type objectType);
    }

    internal class ResultDeserializationStrategyFactory : IResultDeserializationStrategyFactory
    {
        public IResultDeserializationStrategy GetStrategy(Type objectType)
        {
            if (ErrorResultDeserializationStrategy.CanHandle(objectType))
            {
                return ErrorResultDeserializationStrategy.Deserializer;
            }

            if (GenericErrorResultDeserializationStrategy.CanHandle(objectType))
            {
                return GenericErrorResultDeserializationStrategy.Deserializer;
            }

            throw new NotSupportedException($"{nameof(ResultDeserializationStrategyFactory)} doesn't handle given type {objectType.FullName}");
        }

        public bool CanHandle(Type objectType)
        {
            return ErrorResultDeserializationStrategy.CanHandle(objectType)
                   || GenericErrorResultDeserializationStrategy.CanHandle(objectType);
        }
    }
}