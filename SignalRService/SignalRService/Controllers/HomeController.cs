using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace SignalRService.Controllers
{
    
    public class HomeController : BaseController
    {
        private readonly HostingEnvironment _app;
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
            _seed_localization();
            return Json(new { Success = true, Message = "seeding testdata complete" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveLocalization()
        {
            var tpath = Server.MapPath("~");
            tpath += "Localization/LocalizationSrc.xml";
            var respath = Localization.BaseResource.CreateLocalizationSrc(db,tpath);
            return Json(new { Success = true, Message = "localization save to: " + respath }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SeedLocalization()
        {
            try
            {
                List<Models.LocalizationModel> items = new List<Models.LocalizationModel>();
                var fpath = Server.MapPath("~");
                fpath += "Localization\\LocalizationSrc.xml";

                using (var stream = System.IO.File.OpenRead(fpath))
                {
                    var serializer = new XmlSerializer(typeof(List<Models.LocalizationModel>));
                    items = serializer.Deserialize(stream) as List<Models.LocalizationModel>;

                    int maxCounter = items.Count;
                    int counter = 0;
                    foreach (var item in items)
                    {
                        var indb = db.Localization.FirstOrDefault(ln => ln.Key == item.Key && ln.Culture == item.Culture);
                        if(indb == null)
                        {
                            db.Localization.Add(indb);
                        } 
                        else
                        {
                            indb.Value = item.Value;
                        }
                    }
                    db.SaveChanges();
                }

                return Json(new { Success = true, Message = "language seeding complete" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = true, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                throw ex;
            }
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

        public void _seed_localization()
        {
          
        }
    }
}