using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace Hexure.Testing.Logging
{
    public class NUnitLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, NUnitLogger> _loggers = new ConcurrentDictionary<string, NUnitLogger>();

        public void Dispose()
        {
            _loggers.Clear();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new NUnitLogger(name));
        }
    }
}