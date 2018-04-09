using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using SignalRService.Interfaces;
using SignalRService.Localization;

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

        
        public bool LoadSourceToTmpStore(string src, int ownerId)
        {
            try
            {
                var user = userRepository.GetUser(ownerId);
               
                var xe = XElement.Load(src);
                var items = xe.Elements("channel").Elements("item");

                Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("MessageProcessingItems") + " (1 / " + items.Count() + ")", 40, user.SignalRConnections);

                if(items.Count() == 0)
                    return false;

                int total = items.Count();
                int totalcntr = 0;
                int cntr = 0;
                int xlimit = total > 50 ? 50 : 10;

                foreach (var item in items)
                {
                    cntr++;
                    if(cntr == xlimit)
                    {
                        totalcntr = totalcntr + cntr;
                        Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("MessageProcessingItems") + " (" + totalcntr + " / " + total + ")", 45, user.SignalRConnections);
                        cntr = 1;
                    }

                    var pmodel = new Models.ProductImportModel();
                    pmodel.Owner = userContext.GetUser(ownerId);

                    foreach(var element in item.Elements())
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
                                foreach(var sitem in element.Elements() )
                                {
                                    switch(sitem.Name.LocalName)
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
                    _db.ProductTmpImport.Add(pmodel);
 
                    
                }

                Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("MessageProductImportSaving"), 60, user.SignalRConnections);


                _db.SaveChanges();

                Utils.ProgressDialogUtils.Update("productImport", BaseResource.Get("MessageProductImportSaving"), 80, user.SignalRConnections);
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

    }
}