using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{
    public class JSECoinMinerContext
    {
        private readonly DAL.ServiceContext _db;

        public JSECoinMinerContext(DAL.ServiceContext db)
        {
            _db = db;
        }

        public Models.JSECoinMinerConfigurationModel GetDefaultMinerConfig()
        {
            //get the first miner-config - inserted from seeding
            var dbconf = _db.JSECoinMinerConfigurationModels.OrderBy(ln => ln.ID).FirstOrDefault();
            return dbconf;
        }
    }

}