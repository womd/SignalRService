using SignalRService.Hubs;
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
        public MinerStatusData MinerStatus { get; set; }

        private List<string> mGroups;

        public List<string> Groups
        {
            get {
                if (mGroups == null)
                    mGroups = new List<string>();
                return mGroups; }
            set { mGroups = value; }
        }


    }
}