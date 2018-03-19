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

        public ServiceHub()
        {
            orderRepository = new Repositories.OrderRepository(db);
            productRepository = new Repositories.ProductRepository(db);

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

        public List<ViewModels.ProductViewModel>getProducts()
        {
            List<ViewModels.ProductViewModel> reslist = new List<ViewModels.ProductViewModel>();
            var products = productRepository.GetProducts();
            foreach(var item in products)
            {
                reslist.Add(item.ToProductViewModel());
            }
            return reslist;
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
                      OwnerId = 2,
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
           
            var orderViewModel = orderRepository.CheckOrderStatus(orderDTO.OrderId, "Anonymous", "Anonymous");

            if (!string.IsNullOrEmpty(orderViewModel.ErrorMessage))
                return orderViewModel;
            
            switch (orderViewModel.NextMethod)
            {
                case Enums.EnumOrderProcessingMethods.Undefined:
                    break;
                case Enums.EnumOrderProcessingMethods.NewOrder:

                    var orderItems = orderRepository.CheckProducts(orderDTO.Items);
                    if(orderItems.Any(x => x.ErrorNumber != 0))
                    {
                        orderViewModel.ErrorMessage = "ItemsError...";
                        return orderViewModel;
                    }

                    if(orderItems.Count == 0)
                    {
                        orderViewModel.ErrorMessage = "No Items for Order...";
                        return orderViewModel;
                    }
                    var ordervm = orderRepository.AddOrder(orderViewModel, orderItems);
                 
                    GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group).newOrder(ordervm);
                    return ordervm;
                default:
                    break;
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
        public int OrderId { get; set; }
        public List<OrderItem>Items { get; set; }
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