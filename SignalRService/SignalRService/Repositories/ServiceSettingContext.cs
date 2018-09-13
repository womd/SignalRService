using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{
    public class ServiceSettingContext
    {
        private readonly DAL.ServiceContext _db;
        public ServiceSettingContext(DAL.ServiceContext db)
        {
            if(db == null)
            {
                _db = new DAL.ServiceContext();
            }
            else
            {
                _db = db;
            }
        }

        public Models.ServiceSettingModel GetServiceForUrl(string url)
        {
            return _db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == url);
        }

        public Models.ServiceSettingModel GetServiceById(int Id)
        {
            return _db.ServiceSettings.FirstOrDefault(ln => ln.ID == Id);
        }
        
    }
}