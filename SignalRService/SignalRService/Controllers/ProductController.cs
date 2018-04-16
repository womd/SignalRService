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
    public class ProductController : BaseController
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

        public JsonResult List(Hubs.FilterSortConfig config, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<ViewModels.ProductViewModel> products = new List<ViewModels.ProductViewModel>();
                var userClaimPrincipal = User as ClaimsPrincipal;
                int total = 0;
                if (userClaimPrincipal.IsInRole("Admin"))
                {
                    
                    products = productRepository.GetProducts(jtStartIndex, jtPageSize, jtSorting, config);
                    total = productRepository.GetProductsTotal(0);
                }
                else
                {
                    var uservm = userRepository.GetUser(User.Identity.Name);
                    products = productRepository.GetProducts(uservm.Id, jtStartIndex, jtPageSize, jtSorting, config);
                    total = productRepository.GetProductsTotal(uservm.Id);
                }
                return Json(new { Result = "OK", Records = products, TotalRecordCount = total  });
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
            var user = userRepository.GetUser(User.Identity.Name);
            Utils.ProgressDialogUtils.Show("productImport", BaseResource.Get("ProductImportDialogTitle"), BaseResource.Get("ProductImportStarted"), 1, user.SignalRConnections);


            var config = db.ProductImportConfigurations.FirstOrDefault(ln => ln.Id == importConfigId);
            if (config == null)
            {
                Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("ImportConfigNotFound"), 0, user.SignalRConnections);
                return Json(new { Success = false, Message = "config not found.." });
            }

            if (!User.IsInRole("Admin") && config.Owner.ID != user.Id)
            {
                Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("ImportNotOwnerOfConfig"), 0, user.SignalRConnections);
                return Json(new { Success = false, Message = "no permission to load this config..." });
            }

            var importer = Factories.ProductImportFactory.GetProductImportImplementation(Enums.EnumImportType.GoogleProductXML);

            Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("Cleaning"), 0, user.SignalRConnections);
            var prodsToRemove = db.ProductTmpImport.Where(ln => ln.Owner.ID == user.Id).ToList();
            db.ProductTmpImport.RemoveRange(prodsToRemove);
            //int rctr = 0;
            //foreach(var ritem in prodsToRemove)
            //{
            //    rctr++;
            //    Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("CleaningItem") + " (" + rctr + " / " + toCleanCount + ")", Utils.ProductUtils.calc_percent(rctr, toCleanCount), user.SignalRConnections);
            //    db.ProductTmpImport.Remove(ritem);
            //}
            db.SaveChanges();

            if (importer.ImportSource(config.Source, config.Owner.ID))
            {
                    Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("MessageProductImportFinished"), 100, user.SignalRConnections);
                    return Json(new { Success = true, Message = "import completed.." });
            }
            else
            {
                Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("MessageProductImportError"), 0, user.SignalRConnections);
                return Json(new { Success = false, Message = "import failed.." });
            }
        }

        #endregion

        #region product-ajax

        public JsonResult DeleteOwnProducts()
        {
            var user = userRepository.GetUser(User.Identity.Name);
            var ptr = db.Products.Where(ln => ln.Owner.ID == user.Id).ToList();
            db.Products.RemoveRange(ptr);

            var iptr = db.ProductTmpImport.Where(ln => ln.Owner.ID == user.Id).ToList();
            db.ProductTmpImport.RemoveRange(iptr);
            db.SaveChanges();

            Utils.LuceneUtils.ClearLuceneIndex();
            Utils.LuceneUtils.AddUpdateLuceneIndex(db.ProductTmpImport);

            return Json(new { Success = true, Message = "ok, products deleted.." }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteAllProducts()
        {
            if(User.IsInRole("Admin"))
            {
                var ptr = db.Products.ToList();
                db.Products.RemoveRange(ptr);

                var iptr = db.ProductTmpImport.ToList();
                db.ProductTmpImport.RemoveRange(iptr);

                db.SaveChanges();

                Utils.LuceneUtils.ClearLuceneIndex();

                return Json(new { Success = true, Message = "ok, products deleted.." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false, Message = "only admin can do this..." }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}