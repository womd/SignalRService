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

        #region products
        public ProductModel GetProduct(int Id)
        {
            return _db.Products.FirstOrDefault(ln => ln.ID == Id);
        }

        public ProductModel GetProduct(string identifier)
        {
            return _db.Products.FirstOrDefault(ln => ln.ProductIdentifier == identifier);
        }

        public ProductModel AddOrUpdateProduct(ProductModel model)
        {
            ProductModel dbmodel;
            ProductImportModel dbimportmodel = new ProductImportModel();
            if (model.ID == 0)
            {
                //try finding it
                dbmodel = _db.Products.FirstOrDefault(x => x.SrcIdentifier == model.SrcIdentifier && x.Owner.ID == model.Owner.ID);
                if (dbmodel == null)
                {
                    //add product -
                    dbmodel = _db.Products.Add(model);
                    //add import-product for lucene
                    dbimportmodel = _db.ProductTmpImport.Add(new ProductImportModel()
                    {
                        Description = model.Description,
                        Mpn = model.PartNo,
                        ImageLink = model.ImageUrl,
                        Owner = model.Owner,
                        OwnerIdString = model.Owner.ToString(),
                        SrcId = Guid.NewGuid().ToString(),
                        //PriceString = model.Price.ToString(),
                    });

          
                }
            }
            else
            {
                dbmodel = _db.Products.FirstOrDefault(ln => ln.ID == model.ID);
                dbmodel.Description = model.Description;
                dbmodel.Name = model.Name;
                dbmodel.Owner = model.Owner;
                dbmodel.PartNo = model.PartNo;
                dbmodel.Price = model.Price;
                dbmodel.SrcIdentifier = model.SrcIdentifier;
                dbmodel.ImageUrl = model.ImageUrl;

                dbimportmodel = _db.ProductTmpImport.FirstOrDefault(ln => ln.SrcId == model.SrcIdentifier && ln.Owner.ID == model.Owner.ID);
                dbimportmodel.Description = model.Description;
                dbimportmodel.Title = model.Name;
                dbimportmodel.Mpn = model.PartNo;
                dbimportmodel.ImageLink = model.ImageUrl;

            }
            _db.SaveChanges();
            if(dbimportmodel.Id != 0)
                Utils.LuceneUtils.AddUpdateLuceneIndex(dbimportmodel);

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


        private IQueryable<ProductModel>Filter(IQueryable<ProductModel>baseq, Hubs.UiFilter filter)
        {
            switch (filter.Field)
            {
                case "PartNumber":
                    baseq = baseq.Where(ln => ln.PartNo.Contains(filter.Expression));
                    break;
                case "ProductName":
                    baseq = baseq.Where(ln => ln.Name.Contains(filter.Expression));
                    break;
                case "Description":
                    baseq = baseq.Where(ln => ln.Description.Contains(filter.Expression));
                    break;
                case "Owner":
                    baseq = baseq.Where(ln => ln.Owner.IdentityName.Contains(filter.Expression));
                    break;
                case "Price":
                    decimal dprice = decimal.Parse(filter.Expression);
                    baseq = baseq.Where(ln => ln.Price == dprice);
                    break;
                default:
                    break;
            }
            return baseq;
        }

        private IQueryable<ProductModel>Sort(IQueryable<ProductModel>prodq, string sorting)
        {
            if (sorting.IndexOf("Identifier") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    prodq = prodq.OrderBy(ln => ln.ProductIdentifier);
                else
                    prodq = prodq.OrderByDescending(ln => ln.ProductIdentifier);
            }
            if (sorting.IndexOf("PartNumber") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    prodq = prodq.OrderBy(ln => ln.PartNo);
                else
                    prodq = prodq.OrderByDescending(ln => ln.PartNo);
            }
            if (sorting.IndexOf("Name") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    prodq = prodq.OrderBy(ln => ln.Name);
                else
                    prodq = prodq.OrderByDescending(ln => ln.Name);
            }
            if (sorting.IndexOf("Description") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    prodq = prodq.OrderBy(ln => ln.Description);
                else
                    prodq = prodq.OrderByDescending(ln => ln.Description);
            }
            if (sorting.IndexOf("Owner") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    prodq = prodq.OrderBy(ln => ln.Owner.IdentityName);
                else
                    prodq = prodq.OrderByDescending(ln => ln.Owner.IdentityName);
            }
            if (sorting.IndexOf("Price") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                    prodq = prodq.OrderBy(ln => ln.Price);
                else
                    prodq = prodq.OrderByDescending(ln => ln.Price);
            }
            return prodq;

        }

        public List<ProductModel>GetProducts(int userId, int startIndex, int pageSize, string sorting, Hubs.FilterSortConfig config)
        {
            var prodq = _db.Products.Where(ln => ln.Owner.ID == userId);
            if (config.Filters != null)
            {
                foreach (var filter in config.Filters)
                {
                    prodq = Filter(prodq, filter);
                }
            }

            if (!string.IsNullOrEmpty(sorting))
                prodq = Sort(prodq, sorting);
            else
                prodq.OrderBy(ln => ln.ID);

            prodq.Skip(startIndex).Take(pageSize);
           
            return prodq.ToList();
        }

        public List<ProductModel>GetProducts()
        {
            return _db.Products.ToList();
        }

     

        public List<ProductModel> GetProducts(int StartIndex, int PageSize, string sorting, Hubs.FilterSortConfig config)
        {
            IQueryable<ProductModel> prodq = _db.Products;
            if (config.Filters != null)
            {
                foreach (var filter in config.Filters)
                {
                    prodq = Filter(prodq, filter);
                }
            }

            if (!string.IsNullOrEmpty(sorting))
                prodq = Sort(prodq, sorting);
            else
                prodq.OrderBy(ln => ln.ID);

            return prodq.Skip(StartIndex).Take(PageSize).ToList();
        }

        public int GetProductsTotal()
        {
            return _db.Products.Count();
        }

        public int GetProductsTotal(int UserId)
        {
            return _db.Products.Where(ln => ln.Owner.ID == UserId).Count();
        }

        #endregion

        #region productImport
        public List<ProductImportConfigurationModel>GetProductImportConfigurations(int startIndex, int pageSize, string sorting)
        {
            var reslist = new List<ProductImportConfigurationModel>();

            switch (sorting)
            {
                case "Type ASC":
                    reslist = _db.ProductImportConfigurations.OrderBy(ln => ln.Type).Skip(startIndex).Take(pageSize).ToList();
                    break;
                case "TYPE DESC":
                    reslist =_db.ProductImportConfigurations.OrderByDescending(ln => ln.Type).Skip(startIndex).Take(pageSize).ToList();
                    break;
                case "Owner ASC":
                    reslist = _db.ProductImportConfigurations.OrderBy(ln => ln.Owner.IdentityName).Skip(startIndex).Take(pageSize).ToList();
                    break;
                case "Owner DESC":
                    reslist = _db.ProductImportConfigurations.OrderBy(ln => ln.Owner.IdentityName).Skip(startIndex).Take(pageSize).ToList();
                    break;
                case "Source ASC":
                    reslist = _db.ProductImportConfigurations.OrderBy(ln => ln.Source).Skip(startIndex).Take(pageSize).ToList();
                    break;
                case "Source DESC":
                    reslist = _db.ProductImportConfigurations.OrderByDescending(ln => ln.Source).Skip(startIndex).Take(pageSize).ToList();
                    break;
                case "Name ASC":
                    reslist = _db.ProductImportConfigurations.OrderBy(ln => ln.Name).Skip(startIndex).Take(pageSize).ToList();
                    break;
                case "NAME DESC":
                    reslist = _db.ProductImportConfigurations.OrderByDescending(ln => ln.Name).Skip(startIndex).Take(pageSize).ToList();
                    break;
                default:
                    reslist = _db.ProductImportConfigurations.OrderBy(ln => ln.Id).Skip(startIndex).Take(pageSize).ToList();
                    break;
            }

          
            //string query = @"select * from ProductImportConfigurationModels order by " + sorting + @"
            //            offset " + startIndex + @" rows
            //            FETCH NEXT " + pageSize + " rows only";
            //reslist = _db.ProductImportConfigurations.SqlQuery(query).ToList();
            return reslist;
        }

        public List<ProductImportConfigurationModel>GetProductImportConfigurations(int userId, int startIndex, int pageSize, string sorting)
        {
           var allconfigs = GetProductImportConfigurations(startIndex, pageSize, sorting);
           var res = allconfigs.Where(ln => ln.Owner.ID == userId).ToList();
           return res;
        }

        public Models.ProductImportConfigurationModel ProductImportconfigurationAddOrUpdate(Models.ProductImportConfigurationModel model)
        {
            try
            {
                Models.ProductImportConfigurationModel res;
                if (model.Id == 0)
                {
                    res = _db.ProductImportConfigurations.Add(new ProductImportConfigurationModel()
                    {
                        Name = model.Name,
                        Owner = model.Owner,
                        Source = model.Source,
                        Type = model.Type
                    });
                }
                else
                {
                    res = _db.ProductImportConfigurations.FirstOrDefault(ln => ln.Id == model.Id);
                    res.Name = model.Name;
                    res.Owner = model.Owner;
                    res.Source = model.Source;
                    res.Type = model.Type;
                }

                _db.SaveChanges();
                return res;
            }
            catch (Exception ex)
            {
                Utils.SimpleLogger logger = new Utils.SimpleLogger();
                logger.Error(ex.Message);
                return null;
            }
        }

        public bool RemoveImportConfiguration(int Id)
        {
            try
            {
                var toremove =_db.ProductImportConfigurations.FirstOrDefault(ln => ln.Id == Id);
                if (toremove == null)
                    return false;

                _db.ProductImportConfigurations.Remove(toremove);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Utils.SimpleLogger logger = new Utils.SimpleLogger();
                logger.Error(ex.Message);
                return false;
            }
        }

        #endregion
    }
}