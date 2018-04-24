using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Controllers
{
    public class MinerController : Controller
    {
        // GET: Miner
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}