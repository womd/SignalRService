using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class LocationViewModel
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string ConnectionId { get; set; }
    }
}