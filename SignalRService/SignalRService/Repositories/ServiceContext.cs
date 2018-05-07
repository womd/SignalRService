using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{
    public class ServiceContext
    {
        private readonly DAL.ServiceContext _db;
        public ServiceContext(DAL.ServiceContext db)
        {
            _db = db;
        }

        
    }
}