using System;
using System.Linq;
using System.Reflection;
using Hexure.Results;

namespace Hexure.Events.Namespace
{
    public interface IEventNamespaceReader
    {
        Maybe<EventNamespace> GetFromAssemblyOfType<TType>();

        Maybe<EventNamespace> GetFromAssemblyOfType(Type type);
    }

    public class EventNamespaceReader : IEventNamespaceReader
    {
        public Maybe<EventNamespace> GetFromAssemblyOfType<TType>()
        {
            return GetFromAssemblyOfType(typeof(TType));
        }

        public Maybe<EventNamespace> GetFromAssemblyOfType(Type type)
        {
            var eventNamespace = type.Assembly.GetCustomAttributes(typeof(EventNamespace))
                .FirstOrDefault() as EventNamespace;

            return Maybe<EventNamespace>.From(eventNamespace);
        }
    }
}