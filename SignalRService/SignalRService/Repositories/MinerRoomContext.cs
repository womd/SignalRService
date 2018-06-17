using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{
    public class MinerRoomContext
    {
        private readonly DAL.ServiceContext _db;

        public MinerRoomContext(DAL.ServiceContext db)
        {
            _db = db;
        }

      
    }

}