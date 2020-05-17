using Hexure.Events.Namespace;
using Hexure.Results;

namespace Hexure.MassTransit.Events
{
    public static class EventNameProviderErrors
    {
        public static Error.ErrorType UnableToCreateName => new Error.ErrorType(nameof(UnableToCreateName),
            "Unable to create name for {0} message due to missing {1} attribute");
    }

    public interface IEventNameProvider
    {
        Result<string> GetEventName<T>();
    }

    public class EventNamespaceNameProvider : IEventNameProvider
    {
        private readonly IEventNamespaceReader _eventNamespaceReader;

        public EventNamespaceNameProvider(IEventNamespaceReader eventNamespaceReader)
        {
            _eventNamespaceReader = eventNamespaceReader;
        }

        public Result<string> GetEventName<T>()
        {
            var ns = _eventNamespaceReader.GetFromAssemblyOfType<T>();
            if (ns.HasNoValue)
                return Result.Fail<string>(
                    EventNameProviderErrors.UnableToCreateName.Build(typeof(T).FullName, nameof(EventNamespace)));

            return Result.Ok($"{ns.Value.Name}:{typeof(T).Name}");
        }
    }
}