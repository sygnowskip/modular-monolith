using System;

namespace Hexure.Events.Namespace
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class EventNamespace : Attribute
    {
        public EventNamespace(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}