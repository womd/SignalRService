using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalRService.Localization;
using SignalRService.ViewModels;
using SignalRService.Utils;
using System.Globalization;

namespace SignalRService.Controllers
{

    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    [Authorize(Roles = "Admin")]
    public class LocalizationController : BaseController
    {
        private DAL.ServiceContext db;
        private Repositories.LocalizationRepository localizationRepository;

        public LocalizationController()
        {
            db = new DAL.ServiceContext();
            localizationRepository = new Repositories.LocalizationRepository(db);
        }

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

            var toCreate = new Models.LocalizationModel()
            {
                Key = model.Key,
                Value = model.Value,
                Culture = model.Culture,
                LastModDate = DateTime.Now,
                ModUser = User.Identity.Name,
                TranslationStatus = Enums.EnumTranslationStatus.Undefined,
                WasHit = false
            };

            try
            {
                var dbobj = localizationRepository.Create(toCreate, User.Identity.Name);
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
                var dbObj = localizationRepository.Update(model.ToLocalizationModel(), User.Identity.Name);
                Localization.UiResources.Instance.removeFromCache(dbObj.Key, CultureInfo.CurrentCulture.Name);
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
                localizationRepository.Delete(Id);
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