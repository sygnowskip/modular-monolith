using System;
using Hexure.Identifiers.Guid;

namespace ModularMonolith.Payments.Language
{
    public sealed class PaymentId : Identifier
    {
        private PaymentId(Guid value) : base(value)
        {
        }
    }
}