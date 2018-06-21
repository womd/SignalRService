using SignalRService.DTOs;
using SignalRService.Localization;
using SignalRService.Utils;
using SignalRService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace SignalRService.Controllers
{
    [Authorize]
    public class ServiceConfigurationController : BaseController
    {
        private DAL.ServiceContext db;
        private Repositories.MinerContext minerContext;
        private Repositories.ServiceSettingRepositorie serviceSettingRepo;

        public ServiceConfigurationController()
        {
            db = new DAL.ServiceContext();
            minerContext = new Repositories.MinerContext(db);
            serviceSettingRepo = new Repositories.ServiceSettingRepositorie(db);
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List()
        {
            if (!Request.IsAuthenticated)
                return Json(new { Result = "ERROR", Message = "unauthorized" }, JsonRequestBehavior.AllowGet);

            List<Models.ServiceSettingModel> dblist = new List<Models.ServiceSettingModel>();
            List<ServiceSettingViewModel> data = new List<ServiceSettingViewModel>();

            var userClaimPrincipal = User as ClaimsPrincipal;
            if(userClaimPrincipal.IsInRole("Admin"))
                dblist = db.ServiceSettings.ToList();
            else
                dblist = db.ServiceSettings.Where(ln => ln.Owner.IdentityName == User.Identity.Name).ToList();
         
            foreach (var item in dblist)
            {
                data.Add( item.ToServiceSettingViewModel());
            }
            return Json(new { Result = "OK", Records = data }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Create(ServiceSettingViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidData") });

            try
            {
                if(!Utils.ValidationUtils.IsNumbersAndLettersOnly(model.ServiceName)
                    || !Utils.ValidationUtils.IsNumbersAndLettersOnly(model.ServiceUrl))
                {
                    return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidDataOnlyNumbersAndLetters") });
                }

                if(Utils.ValidationUtils.IsDangerousString(model.StripePublishableKey, out int dint)
                    || Utils.ValidationUtils.IsDangerousString(model.StripeSecretKey, out dint))
                {
                    return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidDataDangerousCharacters") });
                }

                if (db.ServiceSettings.Any(ln => ln.ServiceUrl == model.ServiceUrl))
                    return Json(new { Result = "ERROR", Message = BaseResource.Get("ServiceUrlAlreadyTaken") });

                var userdata = db.UserData.FirstOrDefault(ln => ln.IdentityName == User.Identity.Name);
                if (userdata == null)
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

                if (model.StripePublishableKey != string.Empty && model.StripeSecretKey != string.Empty)
                {
                    dbobj.StripeSettings = new List<Models.StripeSettingsModel>();
                    dbobj.StripeSettings.Add(new Models.StripeSettingsModel() { PublishableKey = model.StripePublishableKey, SecretKey = model.StripeSecretKey });
                }

                if(model.MinerClientId != string.Empty && model.MinerScriptUrl != string.Empty)
                {
                    var defaultConfig = minerContext.GetDefaultMinerConfig();
                    dbobj.MinerConfiguration = new Models.MinerConfigurationModel()
                    {
                        ClientId = model.MinerClientId,
                        ScriptUrl = model.MinerScriptUrl,
                        Throttle = defaultConfig.Throttle,
                        StartDelayMs = defaultConfig.StartDelayMs,
                        ReportStatusIntervalMs = defaultConfig.ReportStatusIntervalMs
                    };
                }
                else
                {
                    dbobj.MinerConfiguration = minerContext.GetDefaultMinerConfig();
                }

                if(model.ServiceType == (int)Enums.EnumServiceType.LuckyGameDefault)
                {
               //     var defRule0 = new Models.LuckyGameWinningRule() { AmountMatchingCards = 2, WinFactor = 1.2f };
                    var defRule1 = new Models.LuckyGameWinningRule() { AmountMatchingCards = 3, WinFactor = 1.6f };
                    var defRule2 = new Models.LuckyGameWinningRule() { AmountMatchingCards = 4, WinFactor = 4 };
                    var defRule3 = new Models.LuckyGameWinningRule() { AmountMatchingCards = 5, WinFactor = 5 };
                    var defRule4 = new Models.LuckyGameWinningRule() { AmountMatchingCards = 6, WinFactor = 10 };

                    var gsmodel = new Models.LuckyGameSettingsModel() {
                        MoneyAvailable = 0,
                        WinningRules = new List<Models.LuckyGameWinningRule>()
                         
                    };
                 //   gsmodel.WinningRules.Add(defRule0);
                    gsmodel.WinningRules.Add(defRule1);
                    gsmodel.WinningRules.Add(defRule2);
                    gsmodel.WinningRules.Add(defRule3);
                    gsmodel.WinningRules.Add(defRule4);

                    dbobj.LuckyGameSettings = new List<Models.LuckyGameSettingsModel>();
                    dbobj.LuckyGameSettings.Add(gsmodel);
                }

                if(model.ServiceType == (int)Enums.EnumServiceType.CrowdMiner)
                {
                    if (dbobj.MiningRooms == null)
                        dbobj.MiningRooms = new List<Models.MiningRoomModel>();

                    dbobj.MiningRooms.Add( new Models.MiningRoomModel() {
                             Name = model.ServiceName,
                             Description = "-- -- --",
                             ShowControls = true
                    });
                }
               
                db.SaveChanges();

                return Json(new { Result = "OK", Record = dbobj.ToServiceSettingViewModel() }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string msg = "";
                foreach(var ve in ex.EntityValidationErrors)
                {
                    msg += ve.ValidationErrors.FirstOrDefault().ErrorMessage;
                }
                return Json(new { Result = "ERROR", Message = msg }, JsonRequestBehavior.AllowGet);
            }

          
        }

        public JsonResult Update(ServiceSettingViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidData") });

            if (!Utils.ValidationUtils.IsNumbersAndLettersOnly(model.ServiceName)
            || !Utils.ValidationUtils.IsNumbersAndLettersOnly(model.ServiceUrl))
            {
                return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidDataOnlyNumbersAndLetters") });
            }

            if (Utils.ValidationUtils.IsDangerousString(model.StripePublishableKey, out int dint)
                || Utils.ValidationUtils.IsDangerousString(model.StripeSecretKey, out dint))
            {
                return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidDataDangerousCharacters") });
            }



            try
            {
                var dbObj = db.ServiceSettings.FirstOrDefault(ln => ln.ID == model.Id);
                if(dbObj == null)
                    return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidSettingsId") });

                if(db.ServiceSettings.Any(ln => ln.ServiceUrl == model.ServiceUrl && ln.ID != model.Id))
                    return Json(new { Result = "ERROR", Message = BaseResource.Get("ServiceUrlAlreadyTaken") });

                dbObj.ServiceName = model.ServiceName;
                dbObj.ServiceType = (Enums.EnumServiceType)model.ServiceType;
                dbObj.ServiceUrl = model.ServiceUrl;

                if(dbObj.StripeSettings.Count == 0)
                {
                    if( model.StripePublishableKey != string.Empty && model.StripeSecretKey != string.Empty)
                    {
                        db.StripeSettings.Add(new Models.StripeSettingsModel() { PublishableKey = model.StripePublishableKey, SecretKey = model.StripeSecretKey, Service = dbObj });
                    }
                        
                }
                else
                {
                    if (model.StripePublishableKey != string.Empty && model.StripeSecretKey != string.Empty)
                    {
                        dbObj.StripeSettings.First().PublishableKey = model.StripePublishableKey;
                        dbObj.StripeSettings.First().SecretKey = model.StripeSecretKey;
                    }
                    else
                    {
                        db.StripeSettings.Remove(dbObj.StripeSettings.FirstOrDefault());
                    }
                }

                dbObj.MinerConfiguration.ClientId = model.MinerClientId;

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
                var dbObj = db.ServiceSettings.FirstOrDefault(ln => ln.ID == Id);

                db.StripeSettings.RemoveRange(dbObj.StripeSettings);

                db.ServiceSettings.Remove(dbObj);
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        public FileResult DownloadServiceXML()
        {
            var xmldata = serviceSettingRepo.GetAllServiceDataAsXML();
            string filename = "Services_Export_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToLongTimeString() + ".xml";
            return File(Encoding.ASCII.GetBytes(xmldata), "text/plain", filename);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public bool ImportServiceXML()
        {

            List<ServicesTransferDTO> xmlxontents = new List<ServicesTransferDTO>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<ServicesTransferDTO>));
            foreach (string pfile in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[pfile];
                string fileName = file.FileName;
                fileName = Server.MapPath("~/uploads/" + fileName);
                // file.InputStream .SaveAs(fileName);
                using (XmlReader reader = XmlReader.Create(file.InputStream))
                {
                    xmlxontents.AddRange(((List<ServicesTransferDTO>) serializer.Deserialize(reader)));
                }
            }

            foreach(var item in xmlxontents)
            {
                serviceSettingRepo.ImportService(item);
            }

            return true;
        }
    }
}