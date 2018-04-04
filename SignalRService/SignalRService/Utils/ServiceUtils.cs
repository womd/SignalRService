using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    public class ServiceUtils
    {
        public static bool IsServiceOwner(int serviceId, string IdentityName)
        {
            DAL.ServiceContext db = new DAL.ServiceContext();
            var service = db.ServiceSettings.FirstOrDefault(ln => ln.Id == serviceId);
            if (service == null)
                return false;

            if (service.Owner.IdentityName == IdentityName)
                return true;

            return false;
        }
    }
}