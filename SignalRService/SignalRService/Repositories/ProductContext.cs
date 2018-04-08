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

        public List<ProductModel>GetProducts(int userId, int startIndex, int pageSize, string sorting)
        {
            List<ProductModel> reslist = new List<ProductModel>();
            if(sorting.IndexOf("Identifier") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    reslist = _db.Products.Where(ln => ln.Owner.ID == userId).OrderBy(ln => ln.ProductIdentifier).Skip(startIndex).Take(pageSize).ToList();
                else
                    reslist = _db.Products.Where(ln => ln.Owner.ID == userId).OrderByDescending(ln => ln.ProductIdentifier).Skip(startIndex).Take(pageSize).ToList();
            }
            if(sorting.IndexOf("PartNumber") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    reslist = _db.Products.Where(ln => ln.Owner.ID == userId).OrderBy(ln => ln.PartNo).Skip(startIndex).Take(pageSize).ToList();
                else
                    reslist = _db.Products.Where(ln => ln.Owner.ID == userId).OrderByDescending(ln => ln.PartNo).Skip(startIndex).Take(pageSize).ToList();
            }
            if (sorting.IndexOf("Name") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    reslist = _db.Products.Where(ln => ln.Owner.ID == userId).OrderBy(ln => ln.Name).Skip(startIndex).Take(pageSize).ToList();
                else
                    reslist = _db.Products.Where(ln => ln.Owner.ID == userId).OrderByDescending(ln => ln.Name).Skip(startIndex).Take(pageSize).ToList();
            }
            if (sorting.IndexOf("Description") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    reslist = _db.Products.Where(ln => ln.Owner.ID == userId).OrderBy(ln => ln.Description).Skip(startIndex).Take(pageSize).ToList();
                else
                    reslist = _db.Products.Where(ln => ln.Owner.ID == userId).OrderByDescending(ln => ln.Description).Skip(startIndex).Take(pageSize).ToList();
            }
            if (sorting.IndexOf("Owner") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    reslist = _db.Products.Where(ln => ln.Owner.ID == userId).OrderBy(ln => ln.Owner.IdentityName).Skip(startIndex).Take(pageSize).ToList();
                else
                    reslist = _db.Products.Where(ln => ln.Owner.ID == userId).OrderByDescending(ln => ln.Owner.IdentityName).Skip(startIndex).Take(pageSize).ToList();
            }
            if (sorting.IndexOf("Price") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    reslist = _db.Products.Where(ln => ln.Owner.ID == userId).OrderBy(ln => ln.Price).Skip(startIndex).Take(pageSize).ToList();
                else
                    reslist = _db.Products.Where(ln => ln.Owner.ID == userId).OrderByDescending(ln => ln.Price).Skip(startIndex).Take(pageSize).ToList();
            }
            return reslist;

        }

        public List<ProductModel>GetProducts()
        {
            return _db.Products.ToList();
        }

        public List<ProductModel>GetProducts(int StartIndex, int PageSize, string sorting)
        {
            var products = new List<ProductModel>();

            bool executed = false;
            if(sorting.IndexOf("Identifier") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    products = _db.Products.OrderBy(ln => ln.ProductIdentifier).Skip(StartIndex).Take(PageSize).ToList();
                else
                    products = _db.Products.OrderByDescending(ln => ln.ProductIdentifier).Skip(StartIndex).Take(PageSize).ToList();

                executed = true;
            }

            if (sorting.IndexOf("PartNumber") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    products = _db.Products.OrderBy(ln => ln.PartNo).Skip(StartIndex).Take(PageSize).ToList();
                else
                    products = _db.Products.OrderByDescending(ln => ln.PartNo).Skip(StartIndex).Take(PageSize).ToList();

                executed = true;
            }
            if (!executed)
            {
                string query = @"select * from ProductModels p order by " + sorting + @"
                        offset " + StartIndex + @" rows
                        FETCH NEXT " + PageSize + " rows only";
                products = _db.Products.SqlQuery(query).ToList();
            }
            return products;
        }

    }
}