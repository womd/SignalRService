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

        public ProductModel CreateProduct(ProductViewModel product)
        {
            Models.ProductModel pr = new ProductModel()
            {
                ID = product.Id,
                Name = product.Name,
                Description = product.Description,
                Owner = userContext.GetUser(product.Owner.Id),
                PartNo = product.PartNumber,
                Price = product.Price,
                ProductIdentifier = Guid.NewGuid().ToString()
            };
            return productContext.AddOrUpdateProduct(pr);
        }

        public List<ProductViewModel>GetProducts()
        {
            return  productContext.GetProducts().ToProductViewModels();
        }

        public List<ProductViewModel>GetProducts(int startIndex, int pageSize, string sorting)
        {
            var products = productContext.GetProducts(startIndex, pageSize, sorting);
            return products.ToProductViewModels();
        }

        public List<ViewModels.ProductViewModel> GetProducts(int userId, int startIndex, int pageSize, string sorting)
        {
            var products = productContext.GetProducts(userId, startIndex, pageSize, sorting);
            return products.ToProductViewModels();
        }

    }
}