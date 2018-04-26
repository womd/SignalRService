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
    public static class SignalRServiceUtils
    {
      
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
        public static void SendScriptDataToClient(string connectionId, MvcHtmlString data)
        {
     
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Client(connectionId).clientReceiveWorkData(data);
        }

        public static int RemoveDeadConnections()
        {
            var db = new DAL.ServiceContext();
            var heartBeat = GlobalHost.DependencyResolver.Resolve<ITransportHeartbeat>();
            var conns = heartBeat.GetConnections().Select(x => x.ConnectionId).ToList();

            var rmcnt = 0;
            var deadConnections = db.SignalRConnections.Where(x => !conns.Contains(x.SignalRConnectionId)).ToList();
            rmcnt = deadConnections.Count;

            var dependentMinerstati = new List<Models.MinerStatusModel>();
            foreach(var ditem in deadConnections)
            {
                if (ditem.MinerStatus != null)
                    dependentMinerstati.Add(ditem.MinerStatus);
            }

            db.MinerStatus.RemoveRange(dependentMinerstati);
            db.SignalRConnections.RemoveRange(deadConnections);
            db.SaveChanges();
            return rmcnt;
        }

      

    }
}