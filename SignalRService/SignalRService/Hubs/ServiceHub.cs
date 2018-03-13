using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRService.Extensions;

namespace SignalRService.Hubs
{
    public class ServiceHub : Hub
    {
       
        public ServiceHub()
        {

        }

        public override Task OnConnected()
        {
            string refererUrl = Context.Request.GetHttpContext().Request.ServerVariables["HTTP_REFERER"];
            string remoteIP = Context.Request.GetRemoteIpAddress();

            //  Context.ConnectionId
            if (Context.Request.User.Identity.IsAuthenticated)
            {
                DAL.SignalRConnections.Instance.AddOrUpdate(Context.ConnectionId, Enums.EnumSignalRConnectionState.Connected, refererUrl, remoteIP, Context.Request.User.Identity.Name);
            } else
                DAL.SignalRConnections.Instance.AddOrUpdate(Context.ConnectionId, Enums.EnumSignalRConnectionState.Connected, refererUrl, remoteIP);

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


        public Task JoinGroup(string name)
        {
            return Groups.Add(Context.ConnectionId, name.ToLower());
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }

        public void AddToCart(string itemId, string group)
        {
            //notify all other clients
            //Task.Run(() => GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group.ToLower(), Context.ConnectionId).someoneAddedItemToCart(itemId));
            Task.Run(() => GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.All.someoneAddedItemToCart(itemId));

        }

        public OrderData PlaceOrder(OrderData data, string group)
        {
            data.State = Enums.EnumOrderState.Pending;

            //notify others - notify the master
            Task.Run(() => GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group.ToLower(),Context.ConnectionId).someonePlacedAnOrder(data));
            return data;
        }

        public void MinerReportStatus(MinerStatusData data)
        {
            DAL.SignalRConnections.Instance.UpdateMinerStatusData(Context.ConnectionId, data);
            Task.Run(() => GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.All.minerstatsUpdate(Context.ConnectionId,data));
        }

    }

    public class ClientCallbackData
    {
        public string Method { get; set; }
        public object Parameters { get; set; }
    }

    public class OrderData
    {
        public int OrderId { get; set; }
        public List<OrderItem>Items { get; set; }
        public Enums.EnumOrderState State { get; set; }
    }

    public class OrderItem
    {
        public int ItemId { get; set; }
        public int Amount { get; set; }
    }

    public class MinerStatusData
    {
        public bool running { get; set; }
        public bool onMobile { get; set; }
        public bool wasmEnabled { get; set; }
        public bool isAutoThreads { get; set; }
        public float hps { get; set; }
        public int threads { get; set; }
        public float throttle { get; set; }
        public int hashes { get; set; }

    }
}