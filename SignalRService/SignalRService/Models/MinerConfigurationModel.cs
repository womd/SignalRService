using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class MinerConfigurationModel
    {
        public int ID { get; set; }
        public string ScriptUrl { get; set; }
        public string ClientId { get; set; }
        public float Throttle { get; set; }

    }
}