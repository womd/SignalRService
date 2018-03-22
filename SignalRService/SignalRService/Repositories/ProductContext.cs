using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Models;

namespace SignalRService.Repositories
{
    public class ProductContext
    {
        private readonly DAL.ServiceContext _db;
        public ProductContext(DAL.ServiceContext db)
        {
            _db = db;
        }

        public ProductModel GetProduct(int Id)
        {
            return _db.Products.FirstOrDefault(ln => ln.ID == Id);
        }

        public ProductModel AddOrUpdateProduct(ProductModel model)
        {
            ProductModel dbmodel;
            if(model.ID == 0)
                dbmodel = _db.Products.Add(model);
            else
            {
                dbmodel =_db.Products.FirstOrDefault(ln => ln.ID == model.ID);
                dbmodel.Description = model.Description;
                dbmodel.Name = model.Name;
                dbmodel.Owner = model.Owner;
                dbmodel.PartNo = model.PartNo;
                dbmodel.Price = model.Price;
            }
            _db.SaveChanges();
            return dbmodel;
        }

        public void RemoveProduct(int Id)
        {
            var p =_db.Products.FirstOrDefault(ln => ln.ID == Id);
            if(p != null)
            {
                _db.Products.Remove(p);
                _db.SaveChanges();
            }
        }

        public List<ProductModel>GetProducts(UserDataModel user)
        {
            return _db.Products.Where(ln => ln.Owner.ID == user.ID).ToList();
        }

        public List<ProductModel>GetProducts()
        {
            return _db.Products.ToList();
        }

    }
}