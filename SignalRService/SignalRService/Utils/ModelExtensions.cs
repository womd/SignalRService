using SignalRService.Models;
using SignalRService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    public static class ModelExtensions
    {

        public static MinerConfigurationViewModel ToMinerConfigurationViewModel(this MinerConfigurationModel dbmodel)
        {
            return new MinerConfigurationViewModel() {
                ID = dbmodel.ID,
                ClientId = dbmodel.ClientId,
                ScriptUrl = dbmodel.ScriptUrl,
                Throttle = dbmodel.Throttle.ToString(),
                StartDelayMs = dbmodel.StartDelayMs,
                ReportStatusIntervalMs = dbmodel.ReportStatusIntervalMs
            };
        }

        public static ServiceSettingViewModel ToServiceSettingViewModel(this ServiceSettingModel dbmodel )
        {
            return new ServiceSettingViewModel()
            {
                Id = dbmodel.ID,
                ServiceName = dbmodel.ServiceName,
                ServiceUrl = dbmodel.ServiceUrl,
                ServiceType = (int)dbmodel.ServiceType,
                EnumServiceTpe = dbmodel.ServiceType,

                MinerConfigurationViewModel = dbmodel.MinerConfiguration.ToMinerConfigurationViewModel(),
                MinerClientId = dbmodel.MinerConfiguration.ToMinerConfigurationViewModel().ClientId,
                MinerScriptUrl = dbmodel.MinerConfiguration.ToMinerConfigurationViewModel().ScriptUrl,

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
                },
                User = dbmodel.Owner.ToUserDataViewModel(),
                StripeSecretKey = dbmodel.StripeSettings.Count > 0 ? dbmodel.StripeSettings.First().SecretKey : "",
                StripePublishableKey = dbmodel.StripeSettings.Count > 0 ? dbmodel.StripeSettings.First().PublishableKey : "",
                LuckyGameSettingsViewModel = dbmodel.LuckyGameSettings != null && dbmodel.LuckyGameSettings.Count > 0 ? dbmodel.LuckyGameSettings.First().ToLuckyGameConfigurationViewModel() : new LuckyGameSettingsViewModel() { Id = 0, MoneyAvailable = 0, WinningRules = new List<LuckyGameWinningRuleViewModel>() },
                PositionTrackerConfiguratinViewModel = new PositionTrackerConfigurationViewModel() { Id = dbmodel.ID, SignalRGroup = dbmodel.ServiceUrl.ToLower() },
                CrowdMinerConfigurationViewModel = new CrowdMinerConfigurationViewModel() { Id = dbmodel.MiningRooms.FirstOrDefault() != null ? dbmodel.MiningRooms.FirstOrDefault().Id : 0, SignalRGroup = dbmodel.ServiceUrl.ToLower() }

            };
        }

        public static ViewModels.LuckyGameSettingsViewModel ToLuckyGameConfigurationViewModel(this Models.LuckyGameSettingsModel model)
        {
            return new LuckyGameSettingsViewModel()
            {
                Id = model.ID,
                MoneyAvailable = model.MoneyAvailable,
                WinningRules = model.WinningRules.ToList().ToWinningRuleViewModels(),
                SignalRGroup = model.ServiceSettings.ServiceUrl.ToLower()
            };
        }

        public static List<ViewModels.LuckyGameWinningRuleViewModel>ToWinningRuleViewModels(this List<Models.LuckyGameWinningRule>model)
        {
            List<ViewModels.LuckyGameWinningRuleViewModel> res = new List<LuckyGameWinningRuleViewModel>();
            foreach(var item in model)
            {
                res.Add(item.ToWinningRuleViewModel());
            }
            return res;
        }

        public static ViewModels.LuckyGameWinningRuleViewModel ToWinningRuleViewModel(this Models.LuckyGameWinningRule model)
        {
            return new LuckyGameWinningRuleViewModel()
            {
                 Id = model.ID,
                 AmountMatchingCards = model.AmountMatchingCards,
                 WinFactor = model.WinFactor
                 
            };
        }

        public static Hubs.ProductData ToHtmlEncode(this Hubs.ProductData model)
        {
            return new Hubs.ProductData()
            {
                Id = HttpUtility.HtmlEncode(model.Id),
                Name = HttpUtility.HtmlEncode(model.Name),
                Description = HttpUtility.HtmlEncode(model.Description),
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
            vm.SignalRConnections = model.SignalRConnections != null ? model.SignalRConnections.Select(x => x.SignalRConnectionId).ToList() : new List<string>();
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
                 Owner = model.Owner.ToUserDataViewModel(),
                 PartNumber = model.PartNo,
                 Price = model.Price,
                 Identifier = model.ProductIdentifier,
                 ImageUrl = model.ImageUrl
            };
        }

        public static ViewModels.OrderViewModel ToOrderViewModel(this Models.OrderModel model)
        {
            if (model == null)
                return null;

            var vm = new ViewModels.OrderViewModel() {
                CustomerUser = model.CustomerUser != null ? model.CustomerUser.ToUserDataViewModel() : new UserDataViewModel(),
                StoreUser = model.StoreUser != null ? model.StoreUser.ToUserDataViewModel(): new UserDataViewModel(),
                OrderIdentifier = model.OrderIdentifier,
                OrderState = model.OrderState,
                OrderType = model.OrderType,
                CreationDate = model.CreationDate,
                PaymentState = model.PaymentState,
                ShippingState = model.ShippingState,
                Items = new List<OrderItemViewModel>()
            };
            foreach(var item in model.Items)
            {
                vm.Items.Add(item.ToOrderItemViewModel());
            }
            return vm;
        }

        public static List<ViewModels.OrderViewModel> ToOrderViewModels(this List<Models.OrderModel> models)
        {
            List<ViewModels.OrderViewModel> res = new List<OrderViewModel>();
            foreach(var item in models)
            {
                res.Add(item.ToOrderViewModel());
            }
            return res;
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

        public static ViewModels.LocalizationViewModel ToLocalizationViewModel(this Models.LocalizationModel model)
        {
            return new LocalizationViewModel() {
                Culture = model.Culture,
                Key = model.Key,
                LastModDate = model.LastModDate,
                ModUser = model.ModUser,
                TranslationStatus = model.TranslationStatus,
                Id = model.ID,
                Value = model.Value,
                WasHit = model.WasHit
            };
        }

        public static ViewModels.ProductImportConfigurationViewModel ToProductImportConfigurationViewModel(this Models.ProductImportConfigurationModel model)
        {
            return new ProductImportConfigurationViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Owner = model.Owner.ToUserDataViewModel(),
                Source = model.Source,
                Type = model.Type
            };
        }

        public static List<ViewModels.ProductImportConfigurationViewModel>ToProductImportConfigurationViewModels(this List<Models.ProductImportConfigurationModel> models)
        {
            List<ViewModels.ProductImportConfigurationViewModel> list = new List<ProductImportConfigurationViewModel>();
            foreach(var item in models)
            {
                list.Add(item.ToProductImportConfigurationViewModel());
            }
            return list;
        }

        public static ViewModels.GeneralSettingsViewModel ToGeneralSettingsViewModel(this Models.GeneralSettingsModel model)
        {
            return new GeneralSettingsViewModel()
            {
                Id = model.Id,
                Name = model.GeneralSetting.ToString(),
                Type = model.Type,
                Value = model.Value
            };
        }

    }
}