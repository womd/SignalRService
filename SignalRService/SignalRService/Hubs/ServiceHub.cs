using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRService.Hubs
{
    public class ServiceHub : Hub
    {
        public override Task OnConnected()
        {
            DAL.SignalRConnections.Instance.AddOrUpdate(Context.ConnectionId, Enums.EnumSignalRConnectionState.Connected);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
                DAL.SignalRConnections.Instance.Remove(Context.ConnectionId);
            else
                DAL.SignalRConnections.Instance.AddOrUpdate(Context.ConnectionId, Enums.EnumSignalRConnectionState.Disconnected);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            DAL.SignalRConnections.Instance.AddOrUpdate(Context.ConnectionId, Enums.EnumSignalRConnectionState.Connected);
            return base.OnReconnected();
        }
    }

    public class ClientCallbackData
    {
        public string Method { get; set; }
        public object Parameters { get; set; }
    }
}