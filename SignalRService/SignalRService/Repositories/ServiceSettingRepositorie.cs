using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{
    public class ServiceSettingRepositorie
    {
        private Repositories.ServiceSettingContext context;

        public ServiceSettingRepositorie(DAL.ServiceContext db)
        {
            context = new ServiceSettingContext(db);
        }

      

    }
}