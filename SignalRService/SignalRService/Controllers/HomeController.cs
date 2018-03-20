using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Controllers
{
    
    public class HomeController : Controller
    {
        private DAL.ServiceContext db = new DAL.ServiceContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Seeding()
        {
            return View();
        }

        public ActionResult SeedTestData()
        {
            _seed_testdata();
            return Json(new { Success = true, Message = "seeding testdata complete" }, JsonRequestBehavior.AllowGet);
        }

        private void _seed_testdata()
        {
            var defAccountProp = new Models.AccountPropertiesModel() { UserId = "anonymous" };
            db.AccountProperties.Add(defAccountProp);

            //var defaultUser = new Models.UserDataModel() { IdentityName = "Anonymous" };
            //db.UserData.Add(defaultUser);
            //db.SaveChanges();

            var defaultSetting = new Models.ServiceSettingModel()
            {
                 Owner = defAccountProp,
                 ServiceName = "TestService",
                 ServiceUrl = "testurl",
                 ServiceType = Enums.EnumServiceType.OrderService
            };
            db.ServiceSettings.Add(defaultSetting);
            db.SaveChanges();
        }
    }
}