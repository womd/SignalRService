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
    [Authorize]
    public class OrderController : BaseController
    {
        private DAL.ServiceContext db = new DAL.ServiceContext();
        private Repositories.OrderContext orderContext;

        public OrderController()
        {
            orderContext = new Repositories.OrderContext(db);
        }

        public ActionResult Index()
        {
            return View();
        }

        #region jtable-orderlist

        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                var dbOrders = orderContext.GetOrders(jtStartIndex, jtPageSize, jtSorting);
                return Json(new { Result = "OK", Records = dbOrders.ToOrderViewModels() });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult Delete(string orderIdentifier)
        {
            try
            {
                orderContext.Remove(orderIdentifier);
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