using SignalRService.Localization;
using SignalRService.Utils;
using SignalRService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Controllers
{
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class PreDefinedMinerClientController : BaseController
    {
        private DAL.ServiceContext db;

        public PreDefinedMinerClientController()
        {
            db = new DAL.ServiceContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            if (!Request.IsAuthenticated)
                return Json(new { Result = "ERROR", Message = "unauthorized" }, JsonRequestBehavior.AllowGet);

            var userClaimPrincipal = User as ClaimsPrincipal;
            if (!userClaimPrincipal.IsInRole("Admin"))
            {
                return Json(new { Result = "ERROR", Message = "no permission" }, JsonRequestBehavior.AllowGet);
            }

            List<Models.PredefinedMinerClientModel> dblist = new List<Models.PredefinedMinerClientModel>();
            List<PredefinedMinerClientViewModel> data = new List<PredefinedMinerClientViewModel>();

            dblist = db.PredefinedMinerClients.ToList();
          
            foreach (var item in dblist)
            {
                data.Add(item.ToPreDefinedMinerClientViewModel());
            }
            return Json(new { Result = "OK", Records = data }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Create(PredefinedMinerClientViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidData") });

         
             if(string.IsNullOrWhiteSpace(model.ScriptUrl) || string.IsNullOrWhiteSpace(model.ClientId))
                return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidData") });

            try
            {
               
                if (db.PredefinedMinerClients.Any(ln => ln.Id == model.Id))
                    return Json(new { Result = "ERROR", Message = BaseResource.Get("PredefClientAlreadyExists") });

                var dbobj = db.PredefinedMinerClients.Add(new Models.PredefinedMinerClientModel()
                {
                    ClientId = model.ClientId,
                    ScriptUrl = model.ScriptUrl
                });

                db.SaveChanges();

                return Json(new { Result = "OK", Record = dbobj.ToPreDefinedMinerClientViewModel() }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string msg = "";
                foreach (var ve in ex.EntityValidationErrors)
                {
                    msg += ve.ValidationErrors.FirstOrDefault().ErrorMessage;
                }
                return Json(new { Result = "ERROR", Message = msg }, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult Update(PredefinedMinerClientViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidData") });

            
            try
            {
                var dbObj = db.PredefinedMinerClients.FirstOrDefault(ln => ln.Id == model.Id);
                if (dbObj == null)
                    return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidPredefMinerClientId") });

                if (db.PredefinedMinerClients.Any(ln => ln.ClientId == model.ClientId))
                    return Json(new { Result = "ERROR", Message = BaseResource.Get("PredefClientAlreadyExists") });

                dbObj.ClientId = model.ClientId;
                dbObj.ScriptUrl = model.ScriptUrl;
                db.SaveChanges();

                return Json(new { Result = "OK", Message = "data saved.." });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult Delete(int Id)
        {
            try
            {
                var dbObj = db.PredefinedMinerClients.FirstOrDefault(ln => ln.Id == Id);

                db.PredefinedMinerClients.Remove(dbObj);
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