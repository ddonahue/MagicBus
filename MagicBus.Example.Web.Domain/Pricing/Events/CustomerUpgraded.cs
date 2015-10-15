using System;

namespace MagicBus.Example.Web.Domain.Pricing.Events
{
	public class CustomerUpgraded : IEvent
	{
		public Guid CustomerId { get; set; }
	}
}