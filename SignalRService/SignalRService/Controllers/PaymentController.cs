using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Controllers
{
    public class PaymentController : BaseController
    {
        
        public ActionResult StripeCharge(string stipeEmail, string stripeToken)
        {
            return View();
        }

        public ActionResult StripePay(string token)
        {
            return Json("tadaaa...", JsonRequestBehavior.AllowGet);
        }

    }
}