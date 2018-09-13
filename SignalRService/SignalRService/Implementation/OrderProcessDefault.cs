using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using SignalRService.DTOs;
using SignalRService.Hubs;
using SignalRService.Interfaces;
using SignalRService.Utils;

namespace SignalRService.Implementation
{
    public class OrderProcessDefault : IOrderProcess
    {
        private Repositories.OrderRepository orderRepository;
        private Repositories.ProductRepository productRepository;
        private Repositories.UserRepository userRepository;
        private Repositories.ServiceSettingRepositorie serviceRepository;


        private DAL.ServiceContext db;

        public OrderProcessDefault()
        {
            db = new DAL.ServiceContext();
            orderRepository = new Repositories.OrderRepository(db);
            productRepository = new Repositories.ProductRepository(db);
            serviceRepository = new Repositories.ServiceSettingRepositorie(db);
            userRepository = new Repositories.UserRepository(db);
        }

     
        public ViewModels.OrderViewModel CheckOrder(OrderDataDTO orderDto)
        {

            return new ViewModels.OrderViewModel();
        }


        public ViewModels.OrderViewModel ProcessOrder(ViewModels.OrderViewModel orderViewModel, bool SentFromStoreUser)
        {
            switch (orderViewModel.OrderState)
            {
                case Enums.EnumOrderState.Undef:
                    //its a new order 
                    orderViewModel = orderRepository.AddOrder(orderViewModel.Items, orderViewModel.CustomerUser, orderViewModel.StoreUser);

                    GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Clients(
                        Utils.SignalRServiceUtils.JoinClientLists(orderViewModel.CustomerUser.SignalRConnections, orderViewModel.StoreUser.SignalRConnections)
                        ).updateOrder(orderViewModel);
                    break;
                case Enums.EnumOrderState.ClientPlacedOrder:
                    //the item has been acknowledged from Server
                    if (SentFromStoreUser)
                    {
                        orderViewModel.OrderState = Enums.EnumOrderState.HostConfirmedOrder;
                        orderRepository.UpdateOrderState(orderViewModel.OrderIdentifier, Enums.EnumOrderState.HostConfirmedOrder);

                        //set payment to due
                        orderViewModel.PaymentState = Enums.EnumPaymentState.IsDue;
                        orderRepository.UpdatePaymentState(orderViewModel.OrderIdentifier, Enums.EnumPaymentState.IsDue);

                        GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Clients(
                            Utils.SignalRServiceUtils.JoinClientLists(orderViewModel.CustomerUser.SignalRConnections, orderViewModel.StoreUser.SignalRConnections)
                            ).updateOrder(orderViewModel);
                    }
                    else
                    {
                        orderViewModel.ErrorMessage = "Wrong state .... should not happen...";
                    }
                    break;
                case Enums.EnumOrderState.HostConfirmedOrder:
                    if (SentFromStoreUser)
                    {
                        //store user launched shipping
                        orderViewModel.ShippingState = Enums.EnumShippingState.Launched;
                        orderRepository.UpdateShippingState(orderViewModel.OrderIdentifier, Enums.EnumShippingState.Launched);

                    }
                    else
                    {
                        //the item has been receive-ack by client
                        orderViewModel.ShippingState = Enums.EnumShippingState.Delivered;
                        orderRepository.UpdateShippingState(orderViewModel.OrderIdentifier, Enums.EnumShippingState.Delivered);

                        orderViewModel.OrderState = Enums.EnumOrderState.ClientOrderFinished;
                        orderRepository.UpdateOrderState(orderViewModel.OrderIdentifier, Enums.EnumOrderState.ClientOrderFinished);
                    }
                    GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Clients(
                         Utils.SignalRServiceUtils.JoinClientLists(orderViewModel.CustomerUser.SignalRConnections, orderViewModel.StoreUser.SignalRConnections)
                         ).updateOrder(orderViewModel);

                    break;
                case Enums.EnumOrderState.ClientOrderFinished:

                    break;
                case Enums.EnumOrderState.ServerOrderFinished:
                    //payment has been processed in paymentcontroller,
                    //send update to clients and hosts
                    GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Clients(
                       Utils.SignalRServiceUtils.JoinClientLists(orderViewModel.CustomerUser.SignalRConnections, orderViewModel.StoreUser.SignalRConnections)
                       ).updateOrder(orderViewModel);
                 
                    break;
                case Enums.EnumOrderState.Cancel:
                    break;
                case Enums.EnumOrderState.Error:
                    break;
                default:
                    break;
            }
            return orderViewModel;
        }

        private ViewModels.ProductViewModel StageProduct(ProductData data, string group, string connectionId)
        {
            ViewModels.ProductViewModel resVm = new ViewModels.ProductViewModel();
            int dangerIdx = 0;
            if (string.IsNullOrEmpty(group))
            {
                resVm.ErrorMessage = "stageProduct - no groupname given....";
                resVm.ErrorNumber = 7710;
                return resVm;
            }

            if (Utils.ValidationUtils.IsDangerousString(group, out dangerIdx))
            {
                resVm.ErrorMessage = "invalid groupname given....";
                resVm.ErrorNumber = 7710;
                return resVm;
            }

            List<string> vmessages = new List<string>();
            if (Utils.ProductUtils.IsValidProductData(data, out vmessages))
            {
                var newProduct = productRepository.ProductAddOrUpdate(new ViewModels.ProductViewModel()
                {
                    Name = data.Name,
                    Description = data.Description,
                    Owner = userRepository.GetUserFromSignalR(connectionId),
                    Price = data.Price,
                    PartNumber = data.PartNumber
                });

                GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group).productStaged(newProduct.ToProductViewModel());
                resVm.Identifier = newProduct.ProductIdentifier;
                resVm.Name = newProduct.Name;
                resVm.Owner = newProduct.Owner.ToUserDataViewModel();
                resVm.Price = newProduct.Price;
            }
            resVm.ErrorMessage = vmessages.FirstOrDefault(); ;
            return resVm;
        }

        private List<ViewModels.ProductViewModel> getProducts(int userId, string group, SearchConfig config)
        {
            List<ViewModels.ProductViewModel> reslist = new List<ViewModels.ProductViewModel>();
            var dbService = db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == group);
            if (dbService == null)
                return reslist;

            if (config.SearchTerms.Length == 0)
            {
                foreach (var item in db.Products
                        //     .Where(ln => ln.Owner.ID == dbService.Owner.ID)
                        .OrderBy(ln => ln.Name).Take(10))
                {
                    reslist.Add(item.ToProductViewModel());
                }
                return reslist;
            }

            var searchResItems = Utils.LuceneUtils.Search(config.SearchTerms, userId);




            foreach (var item in searchResItems)
            {
                //if(item.Owner.ID == dbService.Owner.ID)
                reslist.Add(item.ToProductViewModel());
            }

            return reslist;
        }

        private void RemoveProduct(string id, string group, string connectionId)
        {
           

                var user = userRepository.GetUserFromSignalR(connectionId);
                if (productRepository.IsOwner(id, user.Id))
                {
                    if (productRepository.RemoveProduct(id))
                    {
                        GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(group).productRemove(id);
                    }
                    else
                    {
                        //todo: change returntype return object indicating state.
                    }

                }
                
        }

        public DTOs.GeneralHubResponseObject ProcessIncoming(DTOs.GeneralHubRequestObject Request)
        {

            GeneralHubResponseObject result = new GeneralHubResponseObject();

            var jconf = JsonConvert.SerializeObject(Request.RequestData);
            var osRequest = JsonConvert.DeserializeObject<OrderServiceRequesObject>(jconf);

           


            var signalRGroup = serviceRepository.GetSignalRGroup(Request.ServiceId); 

            switch (osRequest.Command)
            {
                case "StageProduct":
                    var spconf = JsonConvert.SerializeObject(osRequest.CommandData);
                    var spdata = JsonConvert.DeserializeObject<Hubs.ProductDataWrap>(spconf);
                    var res = StageProduct(spdata.Product, Request.ServiceId.ToString(), Request.ConnectionId);
                    result.ResponseData = res;
                    if (res.ErrorNumber == 0) {
                        result.Success = true;
                    }
                    else {
                        result.Success = false;
                        result.ErrorMessage = res.ErrorMessage;
                    }
                    break;

                case "LoadProducts":
                    var sconf = JsonConvert.SerializeObject(osRequest.CommandData);
                    var searchConfig = JsonConvert.DeserializeObject<Hubs.SearchConfigWrap>(sconf);
                    var user = userRepository.GetUser(Request.User.Identity.Name);
                    var pres = getProducts(user.Id, signalRGroup, searchConfig.SearchConfig);
                    result.ResponseData = pres;
                    break;

                case "RemoveProduct":
                    var rmspconf = JsonConvert.SerializeObject(osRequest.CommandData);
                    var rmspdata = JsonConvert.DeserializeObject<Hubs.ProductRemoveCommand>(rmspconf);
                    RemoveProduct(rmspdata.Id, signalRGroup, Request.ConnectionId);
                    break;

                default:
                    result.Success = false;
                    result.ErrorMessage = "Unknown/invalid Command";
                    break;
            }

            return result;
        }

    }

   
}