using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRService.Extensions;
using SignalRService.Utils;

namespace SignalRService.Hubs
{
    public class ServiceHub : Hub
    {
        private DAL.ServiceContext db = new DAL.ServiceContext();
        private Repositories.OrderRepository orderRepository;
        private Repositories.ProductRepository productRepository;
        private Repositories.UserRepository userRepository;

        public ServiceHub()
        {
            orderRepository = new Repositories.OrderRepository(db);
            productRepository = new Repositories.ProductRepository(db);
            userRepository = new Repositories.UserRepository(db);
        }

        public override Task OnConnected()
        {
            string refererUrl = Context.Request.GetHttpContext().Request.ServerVariables["HTTP_REFERER"];
            string remoteIP = Context.Request.GetRemoteIpAddress();

            //  Context.ConnectionId
            if (Context.Request.User.Identity.IsAuthenticated)
            {
                db.AddConnection(Context.ConnectionId, refererUrl, remoteIP, Context.Request.User.Identity.Name);

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

        /// <summary>
        /// clientrequest for loading products
        /// </summary>
        /// <returns></returns>
        public List<ViewModels.ProductViewModel> getProducts()
        {
            List<ViewModels.ProductViewModel> reslist = new List<ViewModels.ProductViewModel>();
            var products = productRepository.GetProducts();
            foreach (var item in products)
            {
                reslist.Add(item.ToProductViewModel());
            }
            return reslist;
        }

        public List<ViewModels.OrderViewModel> getOrders(Enums.EnumGuiType guiType)
        {
            //see which user - get orders
            var user = userRepository.GetUserFromSignalR(Context.ConnectionId);
            var orders = orderRepository.GetOrders(user.Id,guiType);
            return orders;
        }

        private ViewModels.ProductViewModel _stageProduct(ProductData data, string group)
        {
            ViewModels.ProductViewModel resVm = new ViewModels.ProductViewModel();
            int dangerIdx = 0;
            if(string.IsNullOrEmpty(group))
            {
                resVm.ErrorMessage = "stageProeuct - no groupname given....";
                resVm.ErrorNumber = 7710;
                return resVm;
            }

            if( Utils.ValidationUtils.IsDangerousString(group, out dangerIdx) )
            {
              
                resVm.ErrorMessage = "invalid groupname given....";
                resVm.ErrorNumber = 7710;
                return resVm;
            }

            List<string> vmessages = new List<string>();
            if (Utils.ProductUtils.IsValidProductData(data, out vmessages))
            {
                var newProduct = productRepository.CreateProduct(new ViewModels.ProductViewModel()
                {
                      Name = data.Name,
                      Description = data.Description,
                      OwnerId = 1,
                      Price = data.Price
                        
                });
                
                GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group).productStaged(newProduct.ToProductViewModel());
                resVm.Id = newProduct.ID;
                resVm.Name = newProduct.Name;
                resVm.OwnerId = newProduct.Owner.ID;
                resVm.Price = newProduct.Price;
            }
            return resVm;
        }

        public async Task<ViewModels.ProductViewModel> StageProduct(ProductData data, string group)
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

        private ViewModels.OrderViewModel _processOrderRequest(OrderDataDTO orderDTO, string group)
        {
           
            var orderViewModel = orderRepository.CheckOrder(orderDTO.OrderIdentifier);
            if(orderViewModel == null)
            {
                var orderItems = orderRepository.CheckProducts(orderDTO.Items);
                var productOwnerId = orderItems.FirstOrDefault().OwnerId; // --!
                var storeUser = userRepository.GetUser(productOwnerId);
                var customerUser = userRepository.GetUserFromSignalR(Context.ConnectionId);
              
                if(storeUser == null || customerUser == null || productOwnerId == -1 || orderItems.Count == 0)
                {
                    return new ViewModels.OrderViewModel()
                    {
                        ErrorMessage = "Invalid Order-Data...",
                    };
                }
                orderViewModel = orderRepository.AddOrder(orderItems, customerUser, storeUser);
                GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Clients(storeUser.SignalRConnections).newOrder(orderViewModel);
                return orderViewModel;
            }
            else
            {
                switch (orderViewModel.OrderState)
                {
                    case Enums.EnumOrderState.Undef:
                        break;
                    case Enums.EnumOrderState.ClientPlacedOrder:
                        //the item has been acknowledged from Server
                        
                        orderViewModel.OrderState = Enums.EnumOrderState.HostConfirmedOrder;
                        orderRepository.UpdateOrderState(orderViewModel.OrderIdentifier, Enums.EnumOrderState.HostConfirmedOrder);
                        GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Clients(orderViewModel.CustomerUser.SignalRConnections).updateOrder(orderViewModel);
                        break;
                    case Enums.EnumOrderState.HostConfirmedOrder:
                        break;
                    case Enums.EnumOrderState.ClientOrderFinished:
                        break;
                    case Enums.EnumOrderState.ServerOrderFinished:
                        break;
                    case Enums.EnumOrderState.Cancel:
                        break;
                    case Enums.EnumOrderState.Error:
                        break;
                    default:
                        break;
                }
            }

        

            return orderViewModel;
        }

        public async Task<ViewModels.OrderViewModel> ProcessOrder(OrderDataDTO data, string group)
        {
             return await Task.Run(() => _processOrderRequest(data, group));
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
        public decimal Price { get; set; }
    }

    public class OrderDataDTO
    {
        public List<OrderItem>Items { get; set; }
        public string OrderIdentifier { get; set; }
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