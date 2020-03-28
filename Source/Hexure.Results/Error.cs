﻿using System;
using Newtonsoft.Json;
using SmartFormat;

namespace Hexure.Results
{
    public class PropertyName
    {
        public static PropertyName Empty(bool allowOverride = true) => new PropertyName(string.Empty, allowOverride);

        public string Value { get; }
        public bool AllowOverride { get; }

        public PropertyName(string value, bool allowOverride)
        {
            Value = value;
            AllowOverride = allowOverride;
        }

        public static implicit operator string(PropertyName propertyName) => propertyName.Value;
    }

    public class Error
    {
        public string Code { get; }
        public string Message { get; }

        [JsonProperty]
        public PropertyName PropertyName { get; private set; } = PropertyName.Empty();

        public Error SetPropertyName(string value, bool allowOverride = true)
        {
            if (!PropertyName.AllowOverride)
                throw new InvalidOperationException($"Cannot override property name for error: {Code}, because its property name {PropertyName} is not overridable.");

            PropertyName = new PropertyName(value, allowOverride);
            return this;
        }

        [JsonConstructor]
        private Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public static ErrorType Create(string prefix, string code, string messageFormat)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentException(nameof(prefix));

            return Create($"{prefix}.{code}", messageFormat);
        }

        public static ErrorType Create(string code, string messageFormat)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException(nameof(code));

            if (string.IsNullOrEmpty(messageFormat))
                throw new ArgumentException(nameof(messageFormat));

            return new ErrorType(code, messageFormat);
        }

        public override string ToString() => $"Code: '{Code}', Message: '{Message}'.";

        public class ErrorType
        {
            public string Code { get; }

            private readonly string _messageFormat;

            public ErrorType(string code, string messageFormat)
            {
                Code = code;
                _messageFormat = messageFormat;
            }

            public Error Build(params object[] args)
            => new Error(Code, Smart.Format(_messageFormat, args));

            public static implicit operator string(ErrorType errorType) => errorType.Code;
        }

        public override bool Equals(object obj)
        {
            var error = obj as Error;

            if (ReferenceEquals(error, null))
                return false;

            if (GetType() != obj.GetType())
                return false;

            return EqualsCore(error);
        }

        private bool EqualsCore(Error error)
        {
            return error.Code == Code
                   && error.Message == Message;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = Code.GetHashCode();
                result = (result * 397) ^ Message.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(Error a, Error b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Error a, Error b)
        {
            return !(a == b);
        }
    }
}