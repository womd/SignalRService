using System;
using System.Collections.Generic;
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

        public void AddToCart(int itemId)
        {
            //notify all other clients
            Task.Run(() => GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.AllExcept(Context.ConnectionId).someoneAddedItemToCart(itemId));
        }

        public OrderData PlaceOrder(OrderData data)
        {
            data.State = Enums.EnumOrderState.Pending;

            //notify others - notify the master
            Task.Run(() => GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.AllExcept(Context.ConnectionId).someonePlacedAnOrder(data));
            return data;
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
}