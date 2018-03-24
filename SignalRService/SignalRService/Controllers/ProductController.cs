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
    public class ProductController : Controller
    {
        private DAL.ServiceContext db = new DAL.ServiceContext();
        private Repositories.ProductContext productContext;

        public ProductController()
        {
            productContext = new Repositories.ProductContext(db);
        }

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        #region productlist-jtable

        public JsonResult List()
        {
            try
            {
                var products = productContext.GetProducts();
                return Json(new { Result = "OK", Records = products.ToProductViewModels() });
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
                productContext.RemoveProduct(Id);
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