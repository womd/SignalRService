using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{
    public class MinerContext
    {
        private readonly DAL.ServiceContext _db;

        public MinerContext(DAL.ServiceContext db)
        {
            _db = db;
        }

        public Models.MinerConfigurationModel GetDefaultMinerConfig()
        {
            //get the first miner-config - inserted from seeding
            var dbconf = _db.MinerConfiurationModels.OrderBy(ln => ln.ID).FirstOrDefault();
            return dbconf;
        }
    }

}