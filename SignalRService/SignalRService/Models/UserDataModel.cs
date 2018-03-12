using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class UserDataModel
    {
        public Enums.EnumSignalRConnectionState ConnectionState { get; set; }
        public string RefererUrl { get; set; }
        public string RemoteIp { get; set; }
        public string UserId { get; set; }
    }
}