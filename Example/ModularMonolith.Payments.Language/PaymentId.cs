using System;
using Hexure.Identifiers.Guid;

namespace ModularMonolith.Payments.Language
{
    public sealed class PaymentId : Identifier
    {
        public PaymentId(Guid value) : base(value)
        {
        }
    }
}