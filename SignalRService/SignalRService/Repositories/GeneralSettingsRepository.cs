using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.ViewModels;
using SignalRService.Utils;

namespace SignalRService.Repositories
{
    public class GeneralSettingsRepository
    {

        private DAL.ServiceContext db;

        public GeneralSettingsRepository(DAL.ServiceContext _db)
        {
            db = _db;
        }

        public GeneralSettingsViewModel Create(Enums.EnumGeneralSetting Name, Enums.EnumSettingType Type, string Value)
        {
            var dbobj = db.GeneralSettings.Add(new Models.GeneralSettingsModel()
            {
                 GeneralSetting = Name,
                 Type = Type,
                 Value = Value
            });

            db.SaveChanges();
            return dbobj.ToGeneralSettingsViewModel();
        }

        public GeneralSettingsViewModel Update(int Id, Enums.EnumGeneralSetting Name, Enums.EnumSettingType Type, string Value)
        {
            var dbobj = db.GeneralSettings.FirstOrDefault(ln => ln.Id == Id);
            dbobj.GeneralSetting = Name;
            dbobj.Type = Type;
            dbobj.Value = Value;
            db.SaveChanges();
            return dbobj.ToGeneralSettingsViewModel();
        }

        public void Delete(int Id)
        {
            var del = db.GeneralSettings.FirstOrDefault(ln => ln.Id == Id);
            db.GeneralSettings.Remove(del);
            db.SaveChanges();
        }

    }
}