using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Models;
using SignalRService.ViewModels;

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
                Owner = userContext.GetUser(product.OwnerId)
            };
            return productContext.AddOrUpdateProduct(pr);
        }

        public List<ProductModel>GetProducts()
        {
            var user = userContext.GetUser(2);
            return user.Products.ToList();
        }
    }
}