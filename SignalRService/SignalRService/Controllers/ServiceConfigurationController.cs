using SignalRService.Localization;
using SignalRService.Utils;
using SignalRService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Controllers
{
    [Authorize]
    public class ServiceConfigurationController : BaseController
    {
        private DAL.ServiceContext db = new DAL.ServiceContext();

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public JsonResult List()
        {
            if (!Request.IsAuthenticated)
                return Json(new { Result = "ERROR", Message = "unauthorized" }, JsonRequestBehavior.AllowGet);

            List<Models.ServiceSettingModel> dblist = new List<Models.ServiceSettingModel>();
            List<ServiceSettingViewModel> data = new List<ServiceSettingViewModel>();

            if (User.IsInRole("Admin"))
                dblist = db.ServiceSettings.ToList();
            else
                dblist = db.ServiceSettings.Where(ln => ln.Owner.IdentityName == User.Identity.Name).ToList();
         
            foreach (var item in dblist)
            {
                data.Add( item.ToServiceSettingViewModel());
            }
            return Json(new { Result = "OK", Records = data }, JsonRequestBehavior.AllowGet);

        }

        [Authorize]
        public JsonResult Create(ServiceSettingViewModel model)
        {
            if (db.ServiceSettings.Any(ln => ln.ServiceUrl == model.ServiceUrl))
                return Json(new { Result = "ERROR", Message = BaseResource.Get("ServiceUrlAlreadyTaken") });

            var userdata = db.UserData.FirstOrDefault(ln => ln.IdentityName == User.Identity.Name);
            if(userdata == null)
            {
                userdata = db.UserData.Add(new Models.UserDataModel()
                {
                    IdentityName = User.Identity.Name
                });
            }    
            
            var dbobj = db.ServiceSettings.Add(new Models.ServiceSettingModel()
            {
                Owner = userdata,
                ServiceName = model.ServiceName,
                ServiceUrl = model.ServiceUrl,
                ServiceType = (Enums.EnumServiceType)model.ServiceType,
             });

            db.SaveChanges();

            return Json(new { Result = "OK", Records = dbobj.ToServiceSettingViewModel() }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult Update(ServiceSettingViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidData") });

            try
            {
                var dbObj = db.ServiceSettings.FirstOrDefault(ln => ln.ID == model.Id);
                if(dbObj == null)
                    return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidSettingsId") });

                if(db.ServiceSettings.Any(ln => ln.ServiceUrl == model.ServiceUrl))
                    return Json(new { Result = "ERROR", Message = BaseResource.Get("ServiceUrlAlreadyTaken") });

                dbObj.ServiceName = model.ServiceName;
                dbObj.ServiceType = (Enums.EnumServiceType)model.ServiceType;
                dbObj.ServiceUrl = model.ServiceUrl;
                db.SaveChanges();

                return Json(new { Result = "OK", Message = "data saved.." });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult Delete(int Id)
        {
            try
            {
                var dbObj = db.ServiceSettings.FirstOrDefault(ln => ln.ID == Id);
                db.ServiceSettings.Remove(dbObj);
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}