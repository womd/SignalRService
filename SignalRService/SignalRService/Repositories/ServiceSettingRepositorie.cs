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

        public ServiceSettingRepositorie(DAL.ServiceContext _db)
        {
            context = new ServiceSettingContext(db);
            db = _db;
        }

        public Models.ServiceSettingModel GetNewService(Enums.EnumServiceType ServiceType, Models.UserDataModel User, string Name)
        {

            var dbobj = new Models.ServiceSettingModel()
            {
                Owner = User,
                ServiceName = Name,
                ServiceUrl = GetUnusedUrlString(6),
                ServiceType = (Enums.EnumServiceType) ServiceType
            };
          
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