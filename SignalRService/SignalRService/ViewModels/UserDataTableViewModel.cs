using SignalRService.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class UserDataTableViewModel
    {
        public string ConnectionId { get; set; }
        public string ConnectionState { get; set; }
        public string RefererUrl { get; set; }
        public string RemoteIp { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }

        public bool MinerIsRunning { get; set; }
        public bool MinerIsMobile { get; set; }
        public float MinerHps { get; set; }
        public float MinerThrottle { get; set; }

        public int NrOfGroups { get; set; }
    }
}