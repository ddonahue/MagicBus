using System;
using System.Web.Mvc;
using MagicBus.Example.Web.Domain.Pricing.DataAccess;

namespace MagicBus.Example.Web.Controllers
{
    public class PricingController : Controller
    {
        public ActionResult CustomerStatus(Guid customerId)
        {
            return Content(API.GetCustomerStatus(customerId));
        }
    }
}