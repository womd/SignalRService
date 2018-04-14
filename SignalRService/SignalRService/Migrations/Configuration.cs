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
            if(!context.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductNameMinLength))
            defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductNameMinLength, Type = Enums.EnumSettingType.Int, Value = "3" });

            if (!context.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductNameMaxLength))
            defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductNameMaxLength, Type = Enums.EnumSettingType.Int, Value = "90" });

            if (!context.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductMinPrice))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductMinPrice, Type = Enums.EnumSettingType.Int, Value = "0" });

            if (!context.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductMaxPrice))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductMaxPrice, Type = Enums.EnumSettingType.Float, Value = "99999.99" });

            if (!context.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductPartNumberMinLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductPartNumberMinLength, Type = Enums.EnumSettingType.Int, Value = "3" });

            if (!context.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductNameMaxLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductPartNumberMaxLength, Type = Enums.EnumSettingType.Int, Value = "12" });

            if (!context.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductDescriptionMinLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductDescriptionMinLength, Type = Enums.EnumSettingType.Int, Value = "3" });

            if (!context.GeneralSettings.Any(ln => ln.GeneralSetting == Enums.EnumGeneralSetting.ProductDescriptionMaxLength))
                defaultStandards.Add(new GeneralSettingsModel() { GeneralSetting = Enums.EnumGeneralSetting.ProductDescriptionMaxLength, Type = Enums.EnumSettingType.Int, Value = "128" });

            context.GeneralSettings.AddRange(defaultStandards);
        }
    }
}