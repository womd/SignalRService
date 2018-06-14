using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Transports;
using SignalRService.Hubs;

namespace SignalRService.Utils
{
    public static class SignalRMinerUtils
    {
      
        public static void SetThrottleForClient(float Throttle, string ConnectionId)
        {
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Client(ConnectionId).miner_setThrottle(Throttle);
        }

        public static void SetThrottleForGroup(float Throttle, string group)
        {
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group).minerRoomSetMinerThrottle(Throttle);
        }

    }
}