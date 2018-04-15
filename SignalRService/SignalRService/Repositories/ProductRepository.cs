using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Models;
using SignalRService.ViewModels;
using SignalRService.Utils;

namespace SignalRService.Repositories
{
    public class ProductRepository
    {
        private ProductContext productContext;
        private UserContext userContext;
        public ProductRepository(DAL.ServiceContext db)
        {
            productContext = new ProductContext(db);
            userContext = new UserContext(db);
        }

        #region products
        public ProductModel ProductAddOrUpdate(ProductViewModel product)
        {
            Models.ProductModel pr = new ProductModel()
            {
                ID = product.Id,
                Name = product.Name,
                Description = product.Description,
                Owner = userContext.GetUser(product.Owner.Id),
                PartNo = product.PartNumber,
                Price = product.Price,
                ProductIdentifier = Guid.NewGuid().ToString(),
                ImageUrl = product.ImageUrl,
                SrcIdentifier = product.SrcIdentifier
            };
            return productContext.AddOrUpdateProduct(pr);
        }

        public List<ProductViewModel>GetProducts()
        {
            return  productContext.GetProducts().ToProductViewModels();
        }

        public List<ProductViewModel>GetProducts(int startIndex, int pageSize, string sorting, Hubs.FilterSortConfig config)
        {
            var products = productContext.GetProducts(startIndex, pageSize, sorting, config);
            return products.ToProductViewModels();
        }

        public int GetProductsTotal(int UserId)
        {
            if (UserId == 0)
                return productContext.GetProductsTotal();
            else
                return productContext.GetProductsTotal(UserId);
        }
        public List<ViewModels.ProductViewModel> GetProducts(int userId, int startIndex, int pageSize, string sorting, Hubs.FilterSortConfig config)
        {
            var products = productContext.GetProducts(userId, startIndex, pageSize, sorting, config);
            return products.ToProductViewModels();
        }
        
        public bool RemoveProductbySrc(string SrcIdentifier)
        {
            return productContext.RemoveProduct(SrcIdentifier);
        }
        
        public bool RemoveProduct(string Identifier)
        {
            return productContext.RemoveProduct(Identifier);
        }

        public bool IsOwner(string ProductIndentifier, int OwnerId)
        {
            return productContext.IsOwner(OwnerId, ProductIndentifier);
        }

        #endregion

        #region productImport
        public List<ViewModels.ProductImportConfigurationViewModel>GetProductImportConfigurations(int startInex, int pageSize, string sorting)
        {
            var ics = productContext.GetProductImportConfigurations(startInex, pageSize, sorting);
            return ics.ToProductImportConfigurationViewModels();
        }

        public List<ViewModels.ProductImportConfigurationViewModel> GetProductImportConfigurations(int userId, int startInex, int pageSize, string sorting)
        {
            var ics = productContext.GetProductImportConfigurations(userId, startInex, pageSize, sorting);
            return ics.ToProductImportConfigurationViewModels();
        }

        public ViewModels.ProductImportConfigurationViewModel CreateOrUpdateImportConfiguration(int Id, int Owner, int Type, string Source, string Name)
        {
            var owner = userContext.GetUser(Owner);
            ProductImportConfigurationModel model = new ProductImportConfigurationModel();
            model.Id = Id;
            model.Name = Name;
            model.Owner = owner;
            model.Source = Source;
            var res = productContext.ProductImportconfigurationAddOrUpdate(model);
            return res.ToProductImportConfigurationViewModel();
        }

        public bool DeleteImportConfiguration(int Id)
        {
            return productContext.RemoveImportConfiguration(Id);
        }

        #endregion

    }
}