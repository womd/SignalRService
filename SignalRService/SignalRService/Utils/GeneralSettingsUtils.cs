﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    public class GeneralSettingsUtils
    {
        public static object GetSettingValue(Enums.EnumGeneralSetting setting)
        {
            DAL.ServiceContext db = new DAL.ServiceContext();
            var dbsetting = db.GeneralSettings.FirstOrDefault(ln => ln.GeneralSetting == setting);
            if(dbsetting != null)
            {
                switch(dbsetting.Type)
                {
                    case Enums.EnumSettingType.Bool:
                        return bool.Parse(dbsetting.Value);
                    case Enums.EnumSettingType.Decimal:
                        return decimal.Parse(dbsetting.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                    case Enums.EnumSettingType.Float:
                        return float.Parse(dbsetting.Value);
                    case Enums.EnumSettingType.Int:
                        return int.Parse(dbsetting.Value);
                    case Enums.EnumSettingType.String:
                        return dbsetting.Value.ToString();
                    default:
                        return null;
                }
            }
            throw new Exceptions.SettingNotFoundException("Setting not found: " + setting.ToString());
            
        }
    }
}