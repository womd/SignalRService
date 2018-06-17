using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{
    public class ServiceSettingRepositorie
    {
        private Repositories.ServiceSettingContext context;
        private DAL.ServiceContext db;

        public ServiceSettingRepositorie(DAL.ServiceContext db)
        {
            context = new ServiceSettingContext(db);
            db = new DAL.ServiceContext();
        }

        public Models.ServiceSettingModel CreateService(Enums.EnumServiceType ServiceType, Models.UserDataModel User, string Name)
        {

            var dbobj = db.ServiceSettings.Add(new Models.ServiceSettingModel()
            {
                Owner = User,
                ServiceName = Name,
                ServiceUrl = GetUnusedUrlString(6),
                ServiceType = (Enums.EnumServiceType) ServiceType
            });
            db.SaveChanges();
            return dbobj;
        }

        private string GetUnusedUrlString(int length)
        {
            bool found = false;
            string result = "";
            while(found == false)
            {
                result = Utils.RandomUtils.GetRandomString(length);
                if (!db.ServiceSettings.Any(ln => ln.ServiceUrl == result))
                    found = true;
            }

            return result;
        }

    }
}