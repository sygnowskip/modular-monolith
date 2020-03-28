using System;
using System.Linq;

namespace Hexure.Results.Extensions
{
    internal static class TypeExtensions
    {
        public static object GetPropertyValue(this Type type, string propertyName, object obj)
        {
            var propertyInfo = type.GetProperties().Single(prop => prop.Name == propertyName);
            return propertyInfo.GetValue(obj);
        }
    }
}