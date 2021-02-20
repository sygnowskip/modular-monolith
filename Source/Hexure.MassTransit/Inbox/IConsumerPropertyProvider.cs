using System;
using System.Reflection;
using Hexure.Results;
using MassTransit;
using MassTransit.Context;
using Microsoft.Extensions.Caching.Memory;

namespace Hexure.MassTransit.Inbox
{
    public interface IConsumerPropertyProvider
    {
        Maybe<PropertyInfo> GetConsumerProperty<TMessage>(ConsumeContext<TMessage> consumeContext)
            where TMessage : class;
    }

    public class ConsumerPropertyProvider : IConsumerPropertyProvider
    {
        private string PropertyInfoKey(Type type) => $"PROPERTY_{type.FullName}";
        private readonly IMemoryCache _memoryCache;

        public ConsumerPropertyProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Maybe<PropertyInfo> GetConsumerProperty<TMessage>(ConsumeContext<TMessage> consumeContext)
            where TMessage : class
        {
            var type = consumeContext.GetType();
            if (!_memoryCache.TryGetValue(PropertyInfoKey(type), out PropertyInfo cachedPropertyInfo))
            {
                var propertyInfo = type.GetProperty(nameof(ConsumerConsumeContextScope<object, object>.Consumer));
                _memoryCache.Set(PropertyInfoKey(type), propertyInfo);
                return propertyInfo;
            }

            return cachedPropertyInfo;
        }
    }
}