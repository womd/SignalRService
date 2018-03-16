using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SignalRService.Utils;

namespace SignalRService.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ServiceController : Controller
    {
        private DAL.ServiceContext db = new DAL.ServiceContext();

       
        public ActionResult Index()
        {
            return View("Index");
        }

        public JsonResult SayHello()
        {
            Utils.SignalRServiceUtils.SayHello();
            return Json("done...", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExecuteCallbackOnClient(string fdata)
        {
            var oParams = new JavaScriptSerializer().Deserialize(fdata, typeof(IEnumerable<string>));
            Hubs.ClientCallbackData data = new Hubs.ClientCallbackData();
            List<string> paramList = oParams as List<string>;

            data.Method = paramList[0];
            paramList.RemoveAt(0);
            data.Parameters = ((object) paramList);

            Utils.SignalRServiceUtils.SendClientCallback(data);
            return Json("done...", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Goto(string url)
        {
            try
            {
                var servicesetting = db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == url);
                if (servicesetting == null)
                    return View("UrlNotFound", url);

                return View(servicesetting.ToServiceSettingViewModel());

            }
            catch (Exception ex)
            {

                return View("Exception",ex);
            }

        }

        public PartialViewResult RenderSignalRBase(ViewModels.ServiceSettingViewModel basemodel)
        {
            var model = new SignalRService.ViewModels.SignalRBaseConfigurationViewModel()
            {

                SinalRGroup = basemodel.ServiceUrl
                
            };
            return PartialView("RenderMiner", model);
        }

        public ActionResult SrcStarter(string url)
        {
            var servicesetting = db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == url);
            if (servicesetting == null)
                return View("UrlNotFound", url);

            return View(servicesetting.ToServiceSettingViewModel());
        }

        public ActionResult Test()
        {
            return View("EXTServiceTest");
        }
    }
}