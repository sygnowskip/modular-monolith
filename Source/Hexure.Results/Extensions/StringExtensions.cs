using System;

namespace Hexure.Results.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string property)
        {
            return Char.ToLowerInvariant(property[0]) + property.Substring(1);
        }
    }
}