using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.DAL;
using System.Data.Entity.Migrations;
using SignalRService.Models;

namespace SignalRService.Migrations
{
    public class Configuration : DbMigrationsConfiguration<ServiceContext>
    {
        public Configuration()
        {
            //AutomaticMigrationsEnabled = true;
            CommandTimeout = Int32.MaxValue;
            ContextKey = "SignalRService.DAL.ServiceContext";
        }

        protected override void Seed(ServiceContext context)
        {
            IList<GeneralSettingsModel> defaultStandards = new List<GeneralSettingsModel>();

            defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductNameMinLength, Type = Enums.EnumSettingType.Int, Value = "3" });
            defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductNameMaxLength, Type = Enums.EnumSettingType.Int, Value = "90" });
            defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductMinPrice, Type = Enums.EnumSettingType.Int, Value = "0" });
            defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductMaxPrice, Type = Enums.EnumSettingType.Float, Value = "99999.99" });
            defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductPartNumberMinLength, Type = Enums.EnumSettingType.Int, Value = "3" });
            defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductPartNumberMaxLength, Type = Enums.EnumSettingType.Int, Value = "12" });

            context.GeneralSettings.AddRange(defaultStandards);
        }
    }
}