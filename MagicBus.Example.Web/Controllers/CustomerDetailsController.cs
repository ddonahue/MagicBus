using System;
using System.Web.Mvc;
using MagicBus.Example.Web.Domain.CustomerDetails.Commands;
using MagicBus.Example.Web.Domain.CustomerDetails.DataAccess;
using MagicBus.Example.Web.Models;

namespace MagicBus.Example.Web.Controllers
{
    public class CustomerDetailsController : Controller
    {
        private readonly IBus bus;

        public CustomerDetailsController(IBus bus)
        {
            this.bus = bus;
        }

        public ActionResult List()
        {
            return PartialView(API.GetAllCustomers());
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public RedirectToRouteResult Create(CustomerDetailsViewModel viewModel)
        {
            var command = new CreateCustomer
                {
                    CustomerId = Guid.NewGuid(),
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName
                };
            bus.Send(command);
            return RedirectToAction("Index", "Home");
        }
    }
}
