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
        private DAL.ServiceContext db = new DAL.ServiceContext();

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
                db.AddConnection(Context.ConnectionId,refererUrl,remoteIP,Context.Request.User.Identity.Name);

            }
            else
            {
                db.AddConnection(Context.ConnectionId, refererUrl, remoteIP);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            db.RemoveConnection(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            db.UpdateConnectionState(Context.ConnectionId, Enums.EnumSignalRConnectionState.Connected);
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

        private List<string>_stageProduct(ProductData data, string group)
        {
            List<string> messages = new List<string>();
            int dangerIdx = 0;
            if(string.IsNullOrEmpty(group))
            {
                messages.Add("no groupname given....");
                return messages;
            }

            if( Utils.ValidationUtils.IsDangerousString(group, out dangerIdx) )
            {
                messages.Add("dangrous string given....");
                return messages;
            }

            
            if (Utils.ProductUtils.IsValidProductData(data, out messages))
            {
                Task.Run(() => GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group).productStaged(data));
            }
            return messages;
        }

        public async Task<List<string>> StageProduct(ProductData data, string group)
        {
            return await Task.Run(() => _stageProduct(data,group));
        }

        public void RequestStageList(string group)
        {
            //inform other orderhosts to restage 
            Task.Run(() => GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group).restageAll(group));
        }

        public void RemoveProduct(string id, string group)
        {
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group).productRemove(id);
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
            db.UpdateMinerState(data, Context.ConnectionId);
        }
    }

    public class ClientCallbackData
    {
        public string Method { get; set; }
        public object Parameters { get; set; }
    }

    public class ProductData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public float Price { get; set; }
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