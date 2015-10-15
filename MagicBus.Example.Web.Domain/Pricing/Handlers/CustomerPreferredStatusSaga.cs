using MagicBus.Example.Web.Domain.CustomerDetails.Events;
using MagicBus.Example.Web.Domain.Pricing.Commands;
using MagicBus.Example.Web.Domain.Purchasing.Events;

namespace MagicBus.Example.Web.Domain.Pricing.Handlers
{
    public class CustomerPreferredStatusSaga : IHandler<CustomerCreated>, IHandler<PurchaseAdded>
    {
	    private readonly IBus bus;

	    public CustomerPreferredStatusSaga(IBus bus)
	    {
		    this.bus = bus;
	    }

	    public void Handle(CustomerCreated message)
        {
            bus.Send(new MakeCustomerStandard { CustomerId = message.CustomerId});
        }

        public void Handle(PurchaseAdded message)
        {
			bus.Send(new UpgradeCustomer { CustomerId = message.CustomerId });
        }
    }
}