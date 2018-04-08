using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRService.Hubs;
using SignalRService.Interfaces;

namespace SignalRService.Implementation
{
    public class OrderProcessDefault : IOrderProcess
    {
        private Repositories.OrderRepository orderRepository;
        private DAL.ServiceContext db;

        public OrderProcessDefault()
        {
            db = new DAL.ServiceContext();
            orderRepository = new Repositories.OrderRepository(db);
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

                        //set payment to due
                        orderViewModel.PaymentState = Enums.EnumPaymentState.IsDue;
                        orderRepository.UpdatePaymentState(orderViewModel.OrderIdentifier, Enums.EnumPaymentState.IsDue);

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


    }

   
}