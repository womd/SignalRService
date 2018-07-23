using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Utils;

namespace SignalRService.Repositories
{
    public class CoinIMPMinerRepository
    {

        private Repositories.CoinIMPMinerContext minerContext;
        private DAL.ServiceContext _db;

        public CoinIMPMinerRepository(DAL.ServiceContext db)
        {
            minerContext = new CoinIMPMinerContext(db);
            _db = db;
        }

        public ViewModels.CoinIMPMinerConfigurationViewModel GetDefaultMinerConfig()
        {
            var dbconf = minerContext.GetDefaultMinerConfig();
            return dbconf.ToCoinIMPMinerConfigurationViewModel();
        }

        public Models.CoinIMPMinerConfigurationModel GetNewMinerConfig(string MinerClientId, string MinerScriptUrl, float MinerThrottle, int MinerStartDelayMs, int MinerReportStatusIntervalMs)
        {
            var minerConfiguration = new Models.CoinIMPMinerConfigurationModel()
            {
                ClientId = MinerClientId,
                ScriptUrl = MinerScriptUrl,
                Throttle = MinerThrottle,
                StartDelayMs = MinerStartDelayMs,
                ReportStatusIntervalMs = MinerReportStatusIntervalMs,
            };

            return minerConfiguration;

        }

    }


}