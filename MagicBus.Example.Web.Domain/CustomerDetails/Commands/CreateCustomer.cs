using System;

namespace MagicBus.Example.Web.Domain.CustomerDetails.Commands
{
    public class CreateCustomer : ICommand
    {
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
