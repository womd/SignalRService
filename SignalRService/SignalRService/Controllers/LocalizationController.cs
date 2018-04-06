using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalRService.Localization;
using SignalRService.ViewModels;
using SignalRService.Utils;

namespace SignalRService.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LocalizationController : BaseController
    {
        private DAL.ServiceContext db = new DAL.ServiceContext();
        
        public ActionResult Index()
        {
            return View();
        }

        #region jtable-grid 

        public JsonResult List()
        {
            try
            {
                List<LocalizationViewModel> resList = new List<LocalizationViewModel>();
                foreach (var item in db.Localization.ToList())
                {
                    resList.Add(item.ToLocalizationViewModel());
                }

                return Json(new { Result = "OK", Records = resList });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult Create(LocalizationViewModel model)
        {
            if(!ModelState.IsValid)
                return Json(new { Result = "ERROR",  Message = BaseResource.Get("InvalidData") });

            try
            {
                var dbobj = db.Localization.Add(new Models.LocalizationModel() {
                    Key = model.Key,
                    Value = model.Value,
                    Culture = model.Culture,
                    LastModDate = DateTime.Now,
                    ModUser = User.Identity.Name,
                    TranslationStatus = Enums.EnumTranslationStatus.Undefined,
                    WasHit = false
                });
                db.SaveChanges();
                return Json(new { Result = "OK", Record = dbobj.ToLocalizationViewModel() });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult Update(LocalizationViewModel model)
        {
            if(!ModelState.IsValid)
                return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidData") });

            try
            {
                var dbObj = db.Localization.FirstOrDefault(ln => ln.ID == model.Id);
                dbObj.Culture = model.Culture;
                dbObj.Key = model.Key;
                dbObj.Value = model.Value;
                dbObj.LastModDate = DateTime.Now;
                dbObj.ModUser = User.Identity.Name;
                dbObj.TranslationStatus = Enums.EnumTranslationStatus.Approved;
                dbObj.WasHit = false;
                db.SaveChanges();
                return Json(new { Result = "OK", Record = dbObj.ToLocalizationViewModel() });

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
                var rmObj = db.Localization.FirstOrDefault(ln => ln.ID == Id);
                db.Localization.Remove(rmObj);
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion
    }
}