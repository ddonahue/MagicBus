using System;

namespace MagicBus.Example.Web.Domain.Purchasing.Events
{
    public class PurchaseAdded : IEvent
    {
        public Guid CustomerId { get; set; }
    }
}
