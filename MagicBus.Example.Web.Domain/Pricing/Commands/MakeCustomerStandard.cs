using System;

namespace MagicBus.Example.Web.Domain.Pricing.Commands
{
    public class MakeCustomerStandard : ICommand
    {
        public Guid CustomerId { get; set; }
    }
}
