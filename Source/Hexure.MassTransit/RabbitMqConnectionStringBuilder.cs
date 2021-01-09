using System;
using System.Web;

namespace Hexure.MassTransit
{
    public static class RabbitMqConnectionStringBuilder
    {
        public static readonly string DefaultProtocol = "amqp";
        public static readonly string DefaultVirtualHost = HttpUtility.UrlEncode("/");
        
        public static string Build(string host, string username, string password)
        {
            return $"{DefaultProtocol}://{HttpUtility.UrlEncode(username)}:{HttpUtility.UrlEncode(password)}@{HostWithoutProtocol()}/{DefaultVirtualHost}";
            
            string HostWithoutProtocol()
            {
                var protocolEnd = "://";
                var protocolEndIndex = host.IndexOf(protocolEnd, StringComparison.Ordinal);
                if (protocolEndIndex == -1)
                    return host;

                return host.Substring(protocolEndIndex + protocolEnd.Length);
            }
        }
    }
}