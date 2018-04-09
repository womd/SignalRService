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

        #region import


        public JsonResult ListImportConfig(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<ViewModels.ProductImportConfigurationViewModel> importConfigs = new List<ProductImportConfigurationViewModel>();
                var userClaimPrincipal = User as ClaimsPrincipal;
                if (userClaimPrincipal.IsInRole("Admin"))
                {
                    importConfigs = productRepository.GetProductImportConfigurations(jtStartIndex, jtPageSize, jtSorting);
                }
                else
                {
                    var uservm = userRepository.GetUser(User.Identity.Name);
                    importConfigs = productRepository.GetProductImportConfigurations(uservm.Id, jtStartIndex, jtPageSize, jtSorting);
                }
                return Json(new { Result = "OK", Records = importConfigs });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult CreateImportConfig(int Owner, int Type, string Source, string Name)
        {
            ViewModels.ProductImportConfigurationViewModel model = new ProductImportConfigurationViewModel();
            model.Owner = userContext.GetUser(Owner).ToUserDataViewModel();
            model.Name = Name;
            model.Source = Source;
            model.Type = (Enums.EnumImportType)Type;

            var resvm = productRepository.CreateOrUpdateImportConfiguration(0, Owner, Type, Source,Name);
            return Json(new { Result = "OK", Record = resvm });
        }

        public JsonResult UpdateImportConfig(int Id, int Owner, int Type, string Source, string Name)
        {
            var resvm = productRepository.CreateOrUpdateImportConfiguration(Id, Owner, Type, Source, Name );
            return Json(new { Result = "OK", Record = resvm });
        }

        public JsonResult DeleteImportConfig(int Id)
        {
            if (productRepository.DeleteImportConfiguration(Id))
                return Json(new { Result = "OK" });
            else
                return Json(new { Result = "ERROR", Message = "Error Deleting item.." });

        }

        public JsonResult ProductImportStart(int importConfigId)
        {

            var config = db.ProductImportConfigurations.FirstOrDefault(ln => ln.Id == importConfigId);

            if (config == null)
                return Json(new { Success = false, Message = "config not found.." });

            var user = userRepository.GetUser(User.Identity.Name);
            if(config.Owner.ID != user.Id)
                return Json(new { Success = false, Message = "no permission to load this config..." });

            var importer = Factories.ProductImportFactory.GetProductImportImplementation(Enums.EnumImportType.GoogleProductXML);

            Utils.ProgressDialogUtils.Show("productImport", BaseResource.Get("ProductImportDialogTitle"), BaseResource.Get("CleaningTmpCache"),1, user.SignalRConnections);

            var prodsToRemove = db.ProductTmpImport.Where(ln => ln.Owner.ID == user.Id).ToList();
            db.ProductTmpImport.RemoveRange(prodsToRemove);
            db.SaveChanges();

            Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("MessageLoadingXML"), 10, user.SignalRConnections);




            if (importer.LoadSourceToTmpStore(config.Source, user.Id))
            {
                Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("MessageProductTMPImportCompleted"), 80, user.SignalRConnections);

                foreach(var item in db.ProductTmpImport.Where(ln => ln.Owner.ID == user.Id).ToList())
                {
                    string[] strprice = item.PriceString.Split(' ');

                    ProductViewModel pr = new ProductViewModel();
                    pr.Name = item.Title;
                    pr.ImageUrl = item.ImageLink;
                    pr.Owner = user;
                    pr.PartNumber = item.Mpn;
                    pr.Price = decimal.Parse( strprice[0] );

                    var res = productRepository.CreateProduct(pr);
                }

                return Json(new { Success = true, Message = "import completed.." });
            }
            else
            {
                Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("MessageProductImportError"), 0, user.SignalRConnections);
                return Json(new { Success = false, Message = "import failed.." });
            }
        }

        #endregion

    }
}