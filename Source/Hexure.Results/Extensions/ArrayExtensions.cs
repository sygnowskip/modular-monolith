﻿using System;

namespace Hexure.Results.Extensions
{
    public static class ArrayExtensions
    {
        public static bool IsNullOrEmpty(this Array array)
        {
            return (array == null || array.Length == 0);
        }
    }
}