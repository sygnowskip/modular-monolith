using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Hexure.Results;

namespace Hexure.Events.Serialization
{
    public interface IEventTypeProvider
    {
        Maybe<Type> GetType(string @namespace, string type);
    }

    public class EventTypeProvider : IEventTypeProvider
    {
        private readonly IReadOnlyDictionary<string, Assembly> _assembliesForNamespace;

        public EventTypeProvider(IReadOnlyDictionary<string, Assembly> assembliesForNamespace)
        {
            _assembliesForNamespace = assembliesForNamespace;
        }

        public Maybe<Type> GetType(string eventNamespace, string eventType)
        {
            if (!_assembliesForNamespace.ContainsKey(eventNamespace))
                return Maybe<Type>.None;

            var type = _assembliesForNamespace[eventNamespace]
                .GetTypes()
                .SingleOrDefault(t => t.Name == eventType);

            return Maybe<Type>.From(type);
        }
    }
}