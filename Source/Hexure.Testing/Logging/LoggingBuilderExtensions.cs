using Microsoft.Extensions.Logging;

namespace Hexure.Testing.Logging
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddNUnitConsoleOutput(this ILoggingBuilder logging)
        {
            logging.AddProvider(new NUnitLoggerProvider());
            return logging;
        }
    }
}