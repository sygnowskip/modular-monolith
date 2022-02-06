using System.Collections.Generic;
using Hexure;
using Hexure.Events;
using Hexure.Time;
using ModularMonolith.Language;
using ModularMonolith.Language.Pricing;
using ModularMonolith.Orders.Domain.ValueObjects;
using ModularMonolith.Orders.Language;
using Stateless;

namespace ModularMonolith.Orders.Domain
{
    public partial class Order : AggregateRoot<OrderId, OrderStatus, OrderActions>
    {
        private Order(ISystemTimeProvider systemTimeProvider) : base(systemTimeProvider)
        {
        }

        private Order(UtcDateTime creationDateTime, ContactData seller, ContactData buyer,
            IReadOnlyCollection<OrderItem> items, Price summary, ISystemTimeProvider systemTimeProvider)
            : this(systemTimeProvider)
        {
            Status = OrderStatus.AwaitingForPayment;
            CreationDateTime = creationDateTime;
            Seller = seller;
            Buyer = buyer;
            Items = items;
            Summary = summary;
        }

        public UtcDateTime CreationDateTime { get; private set; }
        public ContactData Seller { get; private set; }
        public ContactData Buyer { get; private set; }
        public IReadOnlyCollection<OrderItem> Items { get; private set; }
        public Price Summary { get; private set; }
    }
}