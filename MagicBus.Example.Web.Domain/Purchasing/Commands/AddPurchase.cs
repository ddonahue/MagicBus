using System;

namespace MagicBus.Example.Web.Domain.Purchasing.Commands
{
    public class AddPurchase : ICommand
    {
        public Guid PurchaseId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
