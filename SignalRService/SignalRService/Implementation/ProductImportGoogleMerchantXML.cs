using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using SignalRService.Interfaces;
using SignalRService.Localization;
using SignalRService.ViewModels;

namespace SignalRService.Implementation 
{


    public class ProductImportGoogleMerchantXML : IProductImport
    {
        private Repositories.ProductRepository productRepository;
        private Repositories.UserRepository userRepository;
        private Repositories.UserContext userContext;
        private DAL.ServiceContext _db;

        public ProductImportGoogleMerchantXML()
        {
            _db = new DAL.ServiceContext();
            productRepository = new Repositories.ProductRepository(_db);
            userRepository = new Repositories.UserRepository(_db);
            userContext = new Repositories.UserContext(_db);
        }

        private Models.ProductImportModel buildImportModel(XElement xelement,Models.UserDataModel owner)
        {
            var pmodel = new Models.ProductImportModel();
            pmodel.Owner = owner;
            pmodel.OwnerIdString = owner.ID.ToString();
            foreach (var element in xelement.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "id":
                        pmodel.SrcId = element.Value;
                        break;
                    case "title":
                        pmodel.Title = element.Value;
                        break;
                    case "description":
                        pmodel.Description = element.Value;
                        break;
                    case "google_product_category":
                        pmodel.GoogleProductCategory = element.Value;
                        break;
                    case "product_type":
                        pmodel.ProductType = element.Value;
                        break;
                    case "link":
                        pmodel.Link = element.Value;
                        break;
                    case "image_link":
                        pmodel.ImageLink = element.Value;
                        break;
                    case "price":
                        pmodel.PriceString = element.Value;
                        break;
                    case "condition":
                        pmodel.Condition = element.Value;
                        break;
                    case "availability":
                        pmodel.Availabiliby = element.Value;
                        break;
                    case "gtin":
                        pmodel.Gtin = element.Value;
                        break;
                    case "mpn":
                        pmodel.Mpn = element.Value;
                        break;
                    case "brand":
                        pmodel.Brand = element.Value;
                        break;
                    case "custom_label_0":
                        pmodel.CustomLabel0 = element.Value;
                        break;
                    case "custom_label_1":
                        pmodel.CustomLabel1 = element.Value;
                        break;
                    case "custom_label_2":
                        pmodel.CustomLabel2 = element.Value;
                        break;
                    case "custom_label_3":
                        pmodel.CustomLabel3 = element.Value;
                        break;
                    case "custom_label_4":
                        pmodel.CustomLabel4 = element.Value;
                        break;
                    case "guid":
                        pmodel.gGuid = element.Value;
                        break;
                    case "identifier_exists":
                        pmodel.IdentifierExists = bool.Parse(element.Value);
                        break;
                    case "shipping":
                        foreach (var sitem in element.Elements())
                        {
                            switch (sitem.Name.LocalName)
                            {
                                case "country":
                                    pmodel.ShippingCountry = sitem.Value;
                                    break;
                                case "service":
                                    pmodel.ShippingService = sitem.Value;
                                    break;
                                case "price":
                                    pmodel.ShippingPrice = sitem.Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return pmodel;
            
        }
        
        private decimal getPrice(string pricestring)
        {
            string[] prstring = pricestring.Split(' ');
            string pstr = prstring[0].Replace(",", "");
            return decimal.Parse(pstr);
        }

       

        public bool ImportSource(string src, int ownerId)
        {
            try
            {
                var user = userRepository.GetUser(ownerId);
                var dbuser = userContext.GetUser(ownerId);
               
                var xe = XElement.Load(src);
                var items = xe.Elements("channel").Elements("item");

                if(items.Count() == 0)
                    return false;

                int cntr = 0;
                int total = items.Count();
                foreach (var item in items)
                {
                    cntr++;
                    Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("ProcessingItem") + " (" + cntr + " / " + total + ")", Utils.ProductUtils.calc_percent(cntr,total), user.SignalRConnections);

                    var pmodel = buildImportModel(item, dbuser);        
                    _db.ProductTmpImport.Add(pmodel);

                    var existing = _db.Products.FirstOrDefault(ln => ln.SrcIdentifier == pmodel.SrcId && ln.Owner.ID == user.Id);

                    if (existing == null)
                    {
                        _db.Products.Add(new Models.ProductModel()
                        {
                            Name = pmodel.Title,
                            Description = pmodel.Description,
                            ImageUrl = pmodel.ImageLink,
                            Owner = dbuser,
                            PartNo = pmodel.Mpn,
                            Price = getPrice(pmodel.PriceString),
                            SrcIdentifier = pmodel.SrcId,
                            ProductIdentifier = Guid.NewGuid().ToString()
                        });
                         
                    }
                    else
                    {
                        existing.Description = pmodel.Description;
                        existing.ImageUrl = pmodel.ImageLink;
                        existing.Name = pmodel.Title;
                        existing.Owner = pmodel.Owner;
                        existing.PartNo = pmodel.Mpn;
                        existing.Price = getPrice(pmodel.PriceString);
                    }

         
                }

                _db.SaveChanges();

                Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("RecreatingLuceneIndex"), 100, user.SignalRConnections);
                Utils.LuceneUtils.AddUpdateLuceneIndex(_db.ProductTmpImport.Where(ln => ln.Owner.ID == user.Id));
                Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("ProductImportFinished"), 100, user.SignalRConnections);

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }
}