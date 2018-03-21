using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRService.Hubs;

namespace SignalRService.Utils
{
    public static class SignalRServiceUtils
    {
        public static void SayHello()
        {
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.All.hello();
        }

        public static void SendClientCallback(ClientCallbackData data)
        {
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.All.clientCallback(data);
        }

        public static List<string>JoinClientLists(List<string>item1,List<string>item2)
        {
            List<string> clientlist = new List<string>();
            clientlist.AddRange(item1);
            clientlist.AddRange(item2);
            return clientlist;
        }
      
    }
}