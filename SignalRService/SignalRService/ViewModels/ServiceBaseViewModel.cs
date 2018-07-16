using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class ServiceBaseViewModel
    {
        public ViewModels.CoinIMPMinerConfigurationViewModel MinerConfigurationViewModel { get; set; }
        public ViewModels.SignalRBaseConfigurationViewModel SiganlRBaseConfigurationVieModel { get; set; }
    }
}