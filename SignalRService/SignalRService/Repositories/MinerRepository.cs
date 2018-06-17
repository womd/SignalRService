using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{
    public class MinerRepository
    {

        private Repositories.MinerContext minerContext;
        private DAL.ServiceContext _db;

        public MinerRepository(DAL.ServiceContext db)
        {
            minerContext = new MinerContext(db);
            _db = db;
        }

        public ViewModels.MinerConfigurationViewModel GetDefaultMinerConfig()
        {
            //var dbconf = context.GetDefaultMinerConfig();
            //dbconf.to
            return new ViewModels.MinerConfigurationViewModel();
        }

        public Models.MinerConfigurationModel CreateMinerConfig(Models.ServiceSettingModel parentService, string MinerClientId, string MinerScriptUrl, float MinerThrottle, int MinerStartDelayMs, int MinerReportStatusIntervalMs)
        {
            var minerConfiguration = new Models.MinerConfigurationModel()
            {
                ClientId = MinerClientId,
                ScriptUrl = MinerScriptUrl,
                Throttle = MinerThrottle,
                StartDelayMs = MinerStartDelayMs,
                ReportStatusIntervalMs = MinerReportStatusIntervalMs,
            };

            parentService.MinerConfiguration = minerConfiguration;
            _db.SaveChanges();

            return minerConfiguration;

        }

    }


}