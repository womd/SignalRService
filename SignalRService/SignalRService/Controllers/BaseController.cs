using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using SignalRService.Utils;


namespace SignalRService.Controllers
{
    public class BaseController : Controller
    {
        private DAL.ServiceContext db = new DAL.ServiceContext();

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            InitializeCulture();
            return base.BeginExecuteCore(callback, state);
        }

        private void InitializeCulture()
        {
            string cultureName;
            // Attempt to read the culture cookie from Request
            var cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0
                    ? Request.UserLanguages[0]
                    : // obtain it from HTTP header AcceptLanguages
                    null;
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures           
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);

            // Save culture in a cookie
            var cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture") { Value = culture, Expires = DateTime.Now.AddYears(1) };
            }
            Response.Cookies.Add(cookie);
            
            if (Request.UrlReferrer == null)
                return RedirectToAction("Index", "Home");

            var url = Request.UrlReferrer.AbsolutePath;
            return Redirect(url);
        }
    }
}