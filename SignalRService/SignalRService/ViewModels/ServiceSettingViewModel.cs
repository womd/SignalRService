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
        public Enums.EnumServiceType EnumServiceTpe { get; set; }
        public ViewModels.UserDataViewModel User { get; set; }

        public ViewModels.MinerConfigurationViewModel MinerConfigurationViewModel { get; set; }
        public ViewModels.SignalRBaseConfigurationViewModel SiganlRBaseConfigurationVieModel { get; set; }
        public ViewModels.OrderClientConfigurationViewModel OrderClientConfigurationViewModel { get; set; }
        public ViewModels.OrderHostConfigurationViewModel OrderHostConfigurationViewModel { get; set; }
        public ViewModels.LuckyGameSettingsViewModel LuckyGameSettingsViewModel { get; set; } 
        public ViewModels.PositionTrackerConfigurationViewModel PositionTrackerConfiguratinViewModel { get; set; }
        public ViewModels.CrowdMinerConfigurationViewModel CrowdMinerConfigurationViewModel { get; set; }


        public string StripeSecretKey { get; set; }
        public string StripePublishableKey { get; set; }
        public string MinerClientId { get; set; }
        
    }
}