﻿using Microsoft.AspNet.Identity;
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
            Utils.DbSeeder seeder = new Utils.DbSeeder();
            seeder.TestServices();
            return Json(new { Success = true, Message = "seeding testdata complete" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SeedGeneralSettings()
        {
            Utils.DbSeeder seeder = new Utils.DbSeeder();
            seeder.GeneralSettings();
            seeder.DefaultMinerConfiguration();

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

                UiResources.Instance.clearCache();
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

       
    }
}