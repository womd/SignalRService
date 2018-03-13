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
        public string ServiceType { get; set; }
    }
}