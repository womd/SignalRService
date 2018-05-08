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
            _db = db;
        }

        public Models.ServiceSettingModel GetServiceForUrl(string url)
        {
            return _db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == url);
        }
        
    }
}