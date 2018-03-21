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

        public JsonResult ServiceList()
        {
            if (!Request.IsAuthenticated)
                return Json(new { Result = "ERROR", Message = "unauthorized" }, JsonRequestBehavior.AllowGet);

            List<ServiceSettingViewModel> data = new List<ServiceSettingViewModel>();

            var services = db.ServiceSettings.Where(ln => ln.Owner.UserId == Request.LogonUserIdentity.User.Value);
            foreach(var item in services)
            {
                data.Add( item.ToServiceSettingViewModel());
            }

            return Json(new { Result = "OK", Records = data }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ServiceCreate(ServiceSettingViewModel model)
        {
            if (!Request.IsAuthenticated)
                return Json(new { Result = "ERROR", Message = "unauthorized" }, JsonRequestBehavior.AllowGet);

                if(db.ServiceSettings.Any(ln => ln.Owner.UserId == Request.LogonUserIdentity.User.Value && ln.ServiceName == model.ServiceName || ln.ServiceUrl == model.ServiceUrl))
                    return Json(new { Result = "ERROR", Message = "duplicate name/url" }, JsonRequestBehavior.AllowGet);

            var accountPropertie = db.AccountProperties.FirstOrDefault(ln => ln.UserId == Request.LogonUserIdentity.User.Value);
            if(accountPropertie == null)
            {
                accountPropertie = db.AccountProperties.Add(new Models.AccountPropertiesModel()
                {
                    UserId = Request.LogonUserIdentity.User.Value
                });
            }    
            

            var dbobj = db.ServiceSettings.Add(new Models.ServiceSettingModel()
            {
                Owner = accountPropertie,
                ServiceName = model.ServiceName,
                ServiceUrl = model.ServiceUrl,
                ServiceType = (Enums.EnumServiceType.OrderService),
                  
            });

            db.SaveChanges();

            return Json(new { Result = "OK", Records = dbobj.ToServiceSettingViewModel() }, JsonRequestBehavior.AllowGet);
        }
    }
}