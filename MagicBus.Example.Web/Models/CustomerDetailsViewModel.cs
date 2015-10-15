using System;
using System.ComponentModel.DataAnnotations;

namespace MagicBus.Example.Web.Models
{
    public class CustomerDetailsViewModel
    {
        public Guid CustomerId { get; set; }
        [Display(Name = "First Name" )]
        public string FirstName { get; set; }
        [Display(Name = "Last Name" )]
        public string LastName { get; set; }
    }
}