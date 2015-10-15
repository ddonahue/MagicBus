using MagicBus.Example.Web.Domain.Pricing.Events;

namespace MagicBus.Example.Web.Domain.Email.Handlers
{
	public class CustomerUpgradedHandler : IHandler<CustomerUpgraded>
	{
		public void Handle(CustomerUpgraded message)
		{
			// send email to the customer
		}
	}
}
