using System;

namespace Hexure.Time
{
    public interface ISystemTimeProvider
    {
        DateTime UtcNow { get; }
    }

    public class SystemTimeProvider : ISystemTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
