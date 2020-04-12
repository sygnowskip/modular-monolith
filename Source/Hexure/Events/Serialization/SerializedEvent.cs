namespace Hexure.Events.Serialization
{
    public class SerializedEvent
    {
        private SerializedEvent() { }
        internal SerializedEvent(string @namespace, string type, string payload)
        {
            Payload = payload;
            Namespace = @namespace;
            Type = type;
        }

        public string Payload { get; }
        public string Namespace { get; }
        public string Type { get; }
    }
}