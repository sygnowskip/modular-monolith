using System.Linq;
using System.Reflection;
using Hexure.Results;

namespace Hexure.Events.Namespace
{
    public interface IEventNamespaceReader
    {
        Maybe<EventNamespace> GetFromAssemblyOfType<TType>();
    }

    public class EventNamespaceReader : IEventNamespaceReader
    {
        public Maybe<EventNamespace> GetFromAssemblyOfType<TType>()
        {
            var eventNamespace = typeof(TType).Assembly.GetCustomAttributes(typeof(EventNamespace))
                .FirstOrDefault() as EventNamespace;

            return Maybe<EventNamespace>.From(eventNamespace);
        }
    }
}