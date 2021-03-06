﻿using System;
using System.Globalization;
using System.Reflection;

namespace Hexure.Identifiers.Common
{
    internal static class IdentifierActivator
    {
        public static object Create(Type identifierType, object value) => Activator.CreateInstance(
            type: identifierType,
            bindingAttr: BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance,
            binder: null,
            args: new[] { value },
            culture: CultureInfo.InvariantCulture,
            activationAttributes: null);
    }
}