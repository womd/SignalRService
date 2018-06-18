using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class PredefinedMinerClientModel
    {
        public int Id { get; set; }
        public string ScriptUrl { get; set; }
        public string ClientId { get; set; }
    }
}