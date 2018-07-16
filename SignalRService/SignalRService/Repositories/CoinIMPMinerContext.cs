using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{
    public class CoinIMPMinerContext
    {
        private readonly DAL.ServiceContext _db;

        public CoinIMPMinerContext(DAL.ServiceContext db)
        {
            _db = db;
        }

        public Models.CoinIMPMinerConfigurationModel GetDefaultMinerConfig()
        {
            //get the first miner-config - inserted from seeding
            var dbconf = _db.CoinIMPMinerConfiurationModels.OrderBy(ln => ln.ID).FirstOrDefault();
            return dbconf;
        }
    }

}