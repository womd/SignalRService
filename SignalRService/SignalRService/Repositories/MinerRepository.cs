using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{
    public class MinerRepository
    {

        private Repositories.MinerContext context;

        public MinerRepository(DAL.ServiceContext db)
        {
            context = new MinerContext(db);
        }

        public ViewModels.MinerConfigurationViewModel GetDefaultMinerConfig()
        {
            //var dbconf = context.GetDefaultMinerConfig();
            //dbconf.to
            return new ViewModels.MinerConfigurationViewModel();
        }

    }


}