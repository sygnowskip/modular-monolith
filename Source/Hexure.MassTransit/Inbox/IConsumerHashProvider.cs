using System;
using System.Security.Cryptography;
using System.Text;
using Hexure.Results;
using Microsoft.Extensions.Caching.Memory;

namespace Hexure.MassTransit.Inbox
{
    public interface IConsumerHashProvider
    {
        Maybe<string> GetConsumerHash(Type consumerType);
    }

    public class ConsumerHashProvider : IConsumerHashProvider
    {
        private string ConsumerHashKey(Type type) => $"CONSUMER_HASH_{type.FullName}";
        private readonly IMemoryCache _memoryCache;

        public ConsumerHashProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Maybe<string> GetConsumerHash(Type consumerType)
        {
            if (!_memoryCache.TryGetValue(ConsumerHashKey(consumerType), out string cachedConsumerHash))
            {
                var consumerHash = ComputeHash(consumerType);
                _memoryCache.Set(ConsumerHashKey(consumerType), consumerHash);
                return consumerHash;
            }

            return cachedConsumerHash;
        }

        private string ComputeHash(Type consumerType)
        {
            var consumerTypeFullName = consumerType.FullName;
            if (string.IsNullOrWhiteSpace(consumerTypeFullName))
                return null;

            using var md5 = MD5.Create();
            var builder = new StringBuilder();                           

            foreach (var b in md5.ComputeHash(Encoding.UTF8.GetBytes(consumerTypeFullName)))
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }
    }
}