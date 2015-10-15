using System;

namespace MagicBus.Example.Web.Domain.CustomerDetails.Events
{
    public class CustomerCreated : IEvent
    {
        public Guid CustomerId { get; set; }
    }
}
