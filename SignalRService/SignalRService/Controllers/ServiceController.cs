using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Controllers
{
    public class ServiceController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        public JsonResult SayHello()
        {
            Utils.SignalRServiceUtils.SayHello();
            return Json("done...", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExecuteCallbackOnClient()
        {
            Hubs.ClientCallbackData data = new Hubs.ClientCallbackData();
            data.Method = "testmethod";
            data.Parameters = new { param1 = 1, param2 = "test" };
            Utils.SignalRServiceUtils.SendClientCallback(data);
            return Json("done...", JsonRequestBehavior.AllowGet);
        }
    }
}