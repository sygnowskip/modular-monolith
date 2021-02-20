using Hexure.Results;
using MassTransit;

namespace Hexure.MassTransit.Inbox
{
    public interface IConsumerProvider
    {
        Maybe<string> GetConsumer<TMessage>(ConsumeContext<TMessage> consumeContext)
            where TMessage : class;
    }

    public class ConsumerProvider : IConsumerProvider
    {
        private readonly IConsumerPropertyProvider _consumerPropertyProvider;
        private readonly IConsumerHashProvider _consumerHashProvider;

        public ConsumerProvider(IConsumerPropertyProvider consumerPropertyProvider,
            IConsumerHashProvider consumerHashProvider)
        {
            _consumerPropertyProvider = consumerPropertyProvider;
            _consumerHashProvider = consumerHashProvider;
        }

        public Maybe<string> GetConsumer<TMessage>(ConsumeContext<TMessage> consumeContext)
            where TMessage : class
        {
            var property = _consumerPropertyProvider.GetConsumerProperty(consumeContext);
            if (property.HasNoValue)
                return Maybe<string>.None;

            var value = property.Value.GetValue(consumeContext);
            if (value == null)
                return Maybe<string>.None;

            return _consumerHashProvider.GetConsumerHash(value.GetType());
        }
    }
}