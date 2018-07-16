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

        public static JSEMinerConfigurationViewModel ToJSECoinMinerConfigurationViewModel(this JSECoinMinerConfigurationModel dbmodel)
        {
            return new JSEMinerConfigurationViewModel()
            {
                ID = dbmodel.ID,
                ClientId = dbmodel.ClientId,
                SiteId = dbmodel.SiteId,
                SubId = dbmodel.SubId
            };
        }

        public static CoinIMPMinerConfigurationViewModel ToCoinIMPMinerConfigurationViewModel(this CoinIMPMinerConfigurationModel dbmodel)
        {
            return new CoinIMPMinerConfigurationViewModel() {
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
            var res = new ServiceSettingViewModel();

            res.Id = dbmodel.ID;
            res.ServiceName = dbmodel.ServiceName;
            res.ServiceUrl = dbmodel.ServiceUrl;
            res.ServiceType = (int)dbmodel.ServiceType;
            res.EnumServiceTpe = dbmodel.ServiceType;

            res.CoinIMPMinerConfigurationViewModel = dbmodel.CoinIMPMinerConfiguration != null ? dbmodel.CoinIMPMinerConfiguration.ToCoinIMPMinerConfigurationViewModel() : null;
            res.MinerClientId = dbmodel.CoinIMPMinerConfiguration != null ? dbmodel.CoinIMPMinerConfiguration.ClientId : string.Empty;
            res.MinerScriptUrl = dbmodel.CoinIMPMinerConfiguration != null ? dbmodel.CoinIMPMinerConfiguration.ScriptUrl : string.Empty;

            res.JSECoinMinerConfigurationViewModel = dbmodel.JSECoinMinerConfiguration != null ? dbmodel.JSECoinMinerConfiguration.ToJSECoinMinerConfigurationViewModel() : null;

            res.SiganlRBaseConfigurationVieModel = new SignalRService.ViewModels.SignalRBaseConfigurationViewModel()
            {
                   SinalRGroup = dbmodel.ServiceUrl.ToLower()
            };

            res.OrderClientConfigurationViewModel = new SignalRService.ViewModels.OrderClientConfigurationViewModel()
            {
                //  AppendToSelector = "*[data-role=page]"
                AppendToSelector = ".body-content",
                SinalRGroup = dbmodel.ServiceUrl.ToLower()
            };

            res.OrderHostConfigurationViewModel = new SignalRService.ViewModels.OrderHostConfigurationViewModel()
            {
                AppendToSelector = ".body-content",
                SinalRGroup = dbmodel.ServiceUrl.ToLower()
            };

                res.User = dbmodel.Owner.ToUserDataViewModel();
            res.StripeSecretKey = dbmodel.StripeSettings != null && dbmodel.StripeSettings.Count > 0 ? dbmodel.StripeSettings.First().SecretKey : "";
            res.StripePublishableKey = dbmodel.StripeSettings != null && dbmodel.StripeSettings.Count > 0 ? dbmodel.StripeSettings.First().PublishableKey : "";
            res.LuckyGameSettingsViewModel = dbmodel.LuckyGameSettings != null && dbmodel.LuckyGameSettings.Count > 0 ? dbmodel.LuckyGameSettings.First().ToLuckyGameConfigurationViewModel() : new LuckyGameSettingsViewModel() { Id = 0, MoneyAvailable = 0, WinningRules = new List<LuckyGameWinningRuleViewModel>() };
            res.PositionTrackerConfiguratinViewModel = new PositionTrackerConfigurationViewModel() { Id = dbmodel.ID, SignalRGroup = dbmodel.ServiceUrl.ToLower() };
            res.CrowdMinerConfigurationViewModel = new CrowdMinerConfigurationViewModel() { Id = dbmodel.MiningRooms.FirstOrDefault() != null ? dbmodel.MiningRooms.FirstOrDefault().Id : 0, SignalRGroup = dbmodel.ServiceUrl.ToLower() };

            return res;
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

        public static Models.LocalizationModel ToLocalizationModel(this ViewModels.LocalizationViewModel viewModel)
        {
            return new LocalizationModel()
            {
                ID = viewModel.Id,
                Culture = viewModel.Culture,
                Key = viewModel.Key,
                LastModDate = viewModel.LastModDate,
                ModUser = viewModel.ModUser,
                TranslationStatus = viewModel.TranslationStatus,
                Value = viewModel.Value,
                WasHit = viewModel.WasHit
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

       public static ViewModels.PredefinedMinerClientViewModel ToPreDefinedMinerClientViewModel(this Models.PredefinedMinerClientModel model)
        {
            return new PredefinedMinerClientViewModel()
            {
                Id = model.Id,
                ClientId = model.ClientId,
                ScriptUrl = model.ScriptUrl
            };
        }

    }
}