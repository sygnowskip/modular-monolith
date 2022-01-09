using System.Threading.Tasks;
using MassTransit;

namespace ModularMonolith.Tests.Scenarios
{
    public class MessageScenarios
    {
        private readonly IBus _bus;

        public MessageScenarios(IBus bus)
        {
            _bus = bus;
        }

        public MessageScenarios Given() => this;
        public MessageScenarios When() => this;
        public MessageScenarios Then() => this;
        public MessageScenarios And() => this;

        public Task MessageIsPublishedAsync<TMessage>(TMessage message)
        {
            return _bus.Publish(message);
        }
    }
}