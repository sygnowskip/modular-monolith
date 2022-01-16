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
    public partial class Order : Entity, IAggregateRoot<OrderId>
    {
        private readonly ISystemTimeProvider _systemTimeProvider;

        private readonly StateMachine<OrderStatus, OrderActions> _stateMachine;

        protected Order(ISystemTimeProvider systemTimeProvider)
        {
            _systemTimeProvider = systemTimeProvider;
            _stateMachine = new StateMachine<OrderStatus, OrderActions>(() => Status, status => Status = status);

            ConfigureStateMachine();
        }

        public Order(UtcDate creationDate, ContactData seller, ContactData buyer,
            IReadOnlyCollection<OrderItem> items, Price summary, ISystemTimeProvider systemTimeProvider)
            : this(systemTimeProvider)
        {
            Status = OrderStatus.AwaitingForPayment;
            CreationDate = creationDate;
            Seller = seller;
            Buyer = buyer;
            Items = items;
            Summary = summary;
        }

        public OrderId Id { get; }
        public OrderStatus Status { get; private set; }
        public UtcDate CreationDate { get; private set; }
        public ContactData Seller { get; private set; }
        public ContactData Buyer { get; private set; }
        public IReadOnlyCollection<OrderItem> Items { get; private set; }
        public Price Summary { get; private set; }
    }
}