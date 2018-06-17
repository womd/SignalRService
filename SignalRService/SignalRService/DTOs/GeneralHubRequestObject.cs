using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.DTOs
{
    public class GeneralHubRequestObject
    {
        public object RequestData { get; set; }
        public int ServiceId { get; set; }
        public string ConnectionId { get; set; }
        public System.Security.Principal.IPrincipal User { get; set; }

    }
}