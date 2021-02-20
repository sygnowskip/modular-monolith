using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Hexure.Results;
using MassTransit;
using MassTransit.Context;

namespace Hexure.MassTransit.Inbox
{
    public interface IConsumerProvider
    {
        Maybe<string> GetConsumer<TMessage>(ConsumeContext<TMessage> consumeContext)
            where TMessage : class;
    }

    public class ConsumerProvider : IConsumerProvider
    {
        public Maybe<string> GetConsumer<TMessage>(ConsumeContext<TMessage> consumeContext)
            where TMessage : class
        {
            var property = GetConsumerProperty(consumeContext);
            if (property == null)
                return Maybe<string>.None;

            var value = property.GetValue(consumeContext);
            if (value == null)
                return Maybe<string>.None;

            var consumerType = value.GetType().FullName;
            if (string.IsNullOrWhiteSpace(consumerType))
                return Maybe<string>.None;

            using var md5 = MD5.Create();
            var builder = new StringBuilder();                           

            foreach (var b in md5.ComputeHash(Encoding.UTF8.GetBytes(consumerType)))
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }

        private PropertyInfo GetConsumerProperty<TMessage>(ConsumeContext<TMessage> consumeContext)
            where TMessage : class
        {
            return consumeContext.GetType().GetProperty(nameof(ConsumerConsumeContextScope<object, object>.Consumer));
        }
    }
}