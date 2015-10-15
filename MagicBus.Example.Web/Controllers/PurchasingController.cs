using System;
using System.Web.Mvc;
using MagicBus.Example.Web.Domain.Purchasing.Commands;
using MagicBus.Example.Web.Domain.Purchasing.DataAccess;

namespace MagicBus.Example.Web.Controllers
{
    public class PurchasingController : Controller
    {
        private readonly IBus bus;

        public PurchasingController(IBus bus)
        {
            this.bus = bus;
        }

        public ActionResult Count(Guid customerId)
        {
            return Content(API.GetPurchases(customerId));
        }

        [HttpPost]
        public RedirectToRouteResult Add(Guid customerId)
        {
            var command = new AddPurchase { CustomerId = customerId, PurchaseId = Guid.NewGuid() };
            bus.Send(command);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult New(Guid customerId)
        {
            return PartialView("New", customerId);
        }
    }
}
