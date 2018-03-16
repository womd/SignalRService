using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class OrderClientConfigurationViewModel : ServiceSettingViewModel
    {
        public string AppendToSelector { get; set; }
        public string SinalRGroup { get; set; }
    }
}