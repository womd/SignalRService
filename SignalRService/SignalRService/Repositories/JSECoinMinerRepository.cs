using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Utils;

namespace SignalRService.Repositories
{
    public class JSECoinMinerRepository
    {

        private Repositories.JSECoinMinerContext minerContext;
        private DAL.ServiceContext _db;

        public JSECoinMinerRepository(DAL.ServiceContext db)
        {
            minerContext = new JSECoinMinerContext(db);
            _db = db;
        }

        public ViewModels.JSEMinerConfigurationViewModel GetDefaultMinerConfig()
        {
            var dbconf = minerContext.GetDefaultMinerConfig();
            return dbconf.ToJSECoinMinerConfigurationViewModel();
        }

        public Models.JSECoinMinerConfigurationModel GetNewMinerConfig(string MinerClientId, string SiteId, string SubId)
        {
            var minerConfiguration = new Models.JSECoinMinerConfigurationModel()
            {
                ClientId = MinerClientId,
                SiteId = SiteId,
                SubId = SubId
            };

            return minerConfiguration;

        }

    }


}