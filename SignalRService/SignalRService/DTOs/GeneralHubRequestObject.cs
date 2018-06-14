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
    }
}