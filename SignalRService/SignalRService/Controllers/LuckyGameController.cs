using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Controllers
{
    public class LuckyGameController : Controller
    {
        // GET: LuckyGame
        public ActionResult Index()
        {
            return View();
        }
    }
}