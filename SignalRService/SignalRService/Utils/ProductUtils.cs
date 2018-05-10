using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web;

namespace SignalRService.Utils
{
    public static class ProductUtils
    {
        public static int calc_percent(int count, int total)
        {
            var perc = count * 100 / total;
            return perc;
        }

        public static bool IsValidProductData(Hubs.ProductData data, out List<string>messages)
        {

            messages = new List<string>();
            int idanger = 0;

            if (Utils.ValidationUtils.IsDangerousString(data.Id, out idanger))
            {
                messages.Add("Invalid Id given...");
                return false;
            }
            if (string.IsNullOrEmpty(data.Name))
            {
                messages.Add("Name darf nicht leer sein.");
                return false;
            }

            
            if (data.Name.Length <  (int) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductNameMinLength) && data.Name.Length > (int) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductNameMaxLength))
            {
                messages.Add("Name soll " + Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductNameMinLength).ToString() + " - " + Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductNameMaxLength) + " Zeichen haben.");
                return false;
            }

            if (Utils.ValidationUtils.IsDangerousString(data.Name, out idanger))
            {
                messages.Add("Name darf keine gefährlichen Zeichen enthalten.");
                return false;
            }

            if (string.IsNullOrEmpty(data.Description))
            {
                messages.Add("Beschreibung darf nicht leer sein.");
                return false;
            }

            if (data.Description.Length < (int) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductDescriptionMaxLength) && data.Description.Length > (int) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductDescriptionMaxLength) )
            {
                messages.Add("Name soll " + Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductDescriptionMaxLength).ToString() + " - " + Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductDescriptionMaxLength) + " Zeichen haben.");
                return false;
            }

            if (Utils.ValidationUtils.IsDangerousString(data.Description, out idanger))
            {
                messages.Add("Name darf keine gefährlichen Zeichen enthalten.");
                return false;
            }

            var maxPriceVal = Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductMaxPrice);
            var minPriceVal = Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductMinPrice);
            decimal maxPrice = decimal.Parse(maxPriceVal.ToString());
            decimal minPrice = decimal.Parse(minPriceVal.ToString());
            if (data.Price > maxPrice || data.Price < minPrice)
            {
                messages.Add("Preis soll zwischen "+ minPrice + " und "+ maxPrice + " sein.");
                return false;
            }

            if (data.PartNumber != string.Empty)
            {
                if (Utils.ValidationUtils.IsDangerousString(data.PartNumber, out idanger))
                {
                    messages.Add("Teilenummer darf keine gefährlichen Zeichen enthalten.");
                    return false;
                }
            }

            if(data.PartNumber.Length < (int) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductPartNumberMinLength) || data.PartNumber.Length > (int) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductPartNumberMaxLength) )
            {
                messages.Add("Teilenummer soll " + Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductPartNumberMinLength).ToString() + " - " + Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ProductPartNumberMaxLength) + " Zeichen haben.");
                return false;
            }


            return true;
        }
    }
}