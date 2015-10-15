using System;

namespace MagicBus.Example.Web.Domain.CustomerDetails.DataAccess
{
    public class CustomerDetails
    {
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}