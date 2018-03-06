using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Controllers
{
    public class ServiceController : Controller
    {
        public ActionResult SiteLoader()
        {
            return View("Index");
        }

        public JsonResult SayHello()
        {
            Utils.SignalRServiceUtils.SayHello();
            return Json("done...", JsonRequestBehavior.AllowGet);
        }
    }
}