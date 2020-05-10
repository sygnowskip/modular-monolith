using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hexure.Events.Publishing
{
    public interface IEventPublisher
    {
        Task Publish(IEnumerable<IEvent> events);
    }
}