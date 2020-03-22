using System;

namespace ModularMonolith.Tests.Common
{
    public class MonolithApiSettings
    {
        public string Url { get; set; }

        public Uri BaseUrl => new Uri(Url);
    }
}