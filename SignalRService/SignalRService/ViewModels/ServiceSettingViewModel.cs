using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class ServiceSettingViewModel
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string ServiceUrl { get; set; }
        public int ServiceType { get; set; }
        public ViewModels.UserDataViewModel User { get; set; }

        public ViewModels.MinerConfigurationViewModel MinerConfigurationViewModel { get; set; }
        public ViewModels.SignalRBaseConfigurationViewModel SiganlRBaseConfigurationVieModel { get; set; }
        public ViewModels.OrderClientConfigurationViewModel OrderClientConfigurationViewModel { get; set; }
        public ViewModels.OrderHostConfigurationViewModel OrderHostConfigurationViewModel { get; set; }
    }
}