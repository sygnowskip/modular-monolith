using System;
using System.Collections.Generic;
using ModularMonolith.Orders.Language;

namespace ModularMonolith.Contracts.Orders
{
    public class OrderDto
    {
        public long Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreationDateTime { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public long Id { get; set; }
        public Guid ExternalId { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
    }
}