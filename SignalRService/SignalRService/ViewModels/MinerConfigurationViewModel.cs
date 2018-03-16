using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class MinerConfigurationViewModel : ServiceSettingViewModel
    {
        public int ID { get; set; }
        public string ScriptUrl { get; set; }
        public string ClientId { get; set; }
        public string Throttle { get; set; }

        public int StartDelayMs { get; set; }
        public int ReportStatusIntervalMs { get; set; }
    }
}