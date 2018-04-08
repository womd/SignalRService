using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SignalRService.Localization;
using SignalRService.ViewModels;
using SignalRService.Utils;
using System.Security.Claims;

namespace SignalRService.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private DAL.ServiceContext db = new DAL.ServiceContext();
        private Repositories.ProductContext productContext;
        private Repositories.ProductRepository productRepository;
        private Repositories.UserContext userContext;
        private Repositories.UserRepository userRepository;

        public ProductController()
        {
            productContext = new Repositories.ProductContext(db);
            productRepository = new Repositories.ProductRepository(db);
            userContext = new Repositories.UserContext(db);
            userRepository = new Repositories.UserRepository(db);
        }

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        #region productlist-jtable

        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<ViewModels.ProductViewModel> products = new List<ViewModels.ProductViewModel>();
                var userClaimPrincipal = User as ClaimsPrincipal;
                if (userClaimPrincipal.IsInRole("Admin"))
                {
                    
                    products = productRepository.GetProducts(jtStartIndex, jtPageSize, jtSorting);
                }
                else
                {
                    var uservm = userRepository.GetUser(User.Identity.Name);
                    products = productRepository.GetProducts(uservm.Id, jtStartIndex, jtPageSize, jtSorting);
                }
                return Json(new { Result = "OK", Records = products });
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