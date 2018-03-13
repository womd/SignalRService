using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using SignalRService.Utils;

namespace SignalRService.Rest
{
    public class RequestViewModel
    {
        public string ServiceUrl { get; set; }
    }

    public class ResponseViewModel
    {
        public string Script { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorNumber { get; set; }
    }

    public class RestServiceController : ApiController
    {

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.Route("api/sc/initialize")]
        public HttpResponseMessage Initialize([FromUri] RequestViewModel model)
        {
            //http://localhost/api/sc/initialize?ServiceUrl=test
            DAL.ServiceContext db = new DAL.ServiceContext();
            var service = db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == model.ServiceUrl);
            if(service == null)
            {
                return Request.CreateResponse(
                    new ResponseViewModel() {
                        ErrorMessage = "Service for: " + model.ServiceUrl + " not found",
                        ErrorNumber = 1010
                    });
            }

            var script = RenderViewToString("Fake", "Goto", service.ToServiceSettingViewModel());

            return Request.CreateResponse(new ResponseViewModel() {
                Script = script
            });
        }

        public static string RenderViewToString(string controllerName, string viewName, object viewData)
        {
            using (var writer = new StringWriter())
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", controllerName);
                var fakeControllerContext = new ControllerContext(new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://google.com", null), new HttpResponse(null))), routeData, new FakeController());
                var razorViewEngine = new RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(fakeControllerContext, viewName, "", false);

                var viewContext = new ViewContext(fakeControllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writer);
                razorViewResult.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }
        public class FakeController : ControllerBase { protected override void ExecuteCore() { } }
    }
}
