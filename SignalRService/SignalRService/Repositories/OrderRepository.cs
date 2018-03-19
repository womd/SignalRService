using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Utils;

namespace SignalRService.Repositories
{
    public class OrderRepository
    {
        private Repositories.OrderContext orderContext; 
        private Repositories.UserContext userContext;
        private Repositories.ProductContext productContext;

        public OrderRepository(DAL.ServiceContext db)
        {
            orderContext = new OrderContext(db);
            userContext = new UserContext(db);
            productContext = new ProductContext(db);
        }

        public ViewModels.ProductViewModel CheckProduct(Hubs.OrderItem itemDTO )
        {
            ViewModels.ProductViewModel vm = new ViewModels.ProductViewModel();
            var product = productContext.GetProduct(itemDTO.ItemId);
            if(product == null)
            {
                vm.ErrorNumber = 1010;
                vm.ErrorMessage = "Could not find product..";
                return vm;
            }

            vm.Description = product.Description;
            vm.Id = product.ID;
            vm.Name = product.Name;
            vm.OwnerId = product.Owner.ID;
            vm.PartNumber = product.PartNo;
            vm.Price = product.Price;

            return vm;

        }

        public List<ViewModels.ProductViewModel> CheckProducts(List<Hubs.OrderItem>items)
        {
            List<ViewModels.ProductViewModel> res = new List<ViewModels.ProductViewModel>();
            foreach (var item in items)
            {
                res.Add(CheckProduct(item));
            }
            return res;
        }

        /// <summary>
        /// determines Orders processing-state
        /// </summary>
        /// <param name="orderIdentifier"></param>
        /// <returns></returns>
        public ViewModels.OrderViewModel CheckOrderStatus(int orderId, string customerIdenfier, string storeIdentifier)
        {
            Models.OrderModel order;
            Interfaces.IOrderProcess orderProcessFactory;
            Enums.EnumOrderProcessingMethods nextMethod;

            order = orderContext.GetOrder(orderId);
            if(order == null)
            {
                orderProcessFactory = Factories.OrderProcessFactory.GetOrderProcessImplementation(Enums.EnumOrderType.Default);
                nextMethod = orderProcessFactory.GetNexProcess(Enums.EnumOrderState.ClientRequestOrderPlacement);
                //its a new order
                order = orderContext.AddOrUpdateOrder(new Models.OrderModel()
                {
                    CustomerUser = userContext.GetUser(customerIdenfier),
                    StoreUser = userContext.GetUser(storeIdentifier),
                    OrderState = Enums.EnumOrderState.ClientRequestOrderPlacement,
                    OrderType = Enums.EnumOrderType.Default,
                    OrderIdentifier = Guid.NewGuid().ToString()
                });

                return buildOrderViewModel(order, nextMethod);
            }

            if(order.CustomerUser.IdentityName != customerIdenfier ||
                order.StoreUser.IdentityName != storeIdentifier)
            {
                return buildOrderViewModel("Customer / Store mismatch...");
            }

            orderProcessFactory = Factories.OrderProcessFactory.GetOrderProcessImplementation(order.OrderType);
            nextMethod = orderProcessFactory.GetNexProcess(order.OrderState);

            order.CustomerUser = userContext.GetUser(order.CustomerUser.IdentityName);
            order.StoreUser = userContext.GetUser(order.StoreUser.IdentityName);

            return buildOrderViewModel(order, nextMethod);
        }

        public ViewModels.OrderViewModel AddOrder(ViewModels.OrderViewModel ovm, List<ViewModels.ProductViewModel>items)
        {
            List<Models.OrderItemModel> orderItems = new List<Models.OrderItemModel>();
            foreach (var item in items)
            {
                orderItems.Add(new Models.OrderItemModel()
                {
                    PartNo = item.PartNumber,
                    Price = item.Price,
                    Name = item.Name,

                });
            }

            var newObj = orderContext.AddOrUpdateOrder(new Models.OrderModel() {
                CustomerUser = userContext.GetUser(ovm.CustomerUser.Name),
                StoreUser = userContext.GetUser(ovm.CustomerUser.Name),
                OrderIdentifier = Guid.NewGuid().ToString(),
                OrderState = Enums.EnumOrderState.ClientPlacedOrder,
                OrderType = Enums.EnumOrderType.Default,
                Items = orderItems
            });

            return newObj.ToOrderViewModel();
        }

        private ViewModels.OrderViewModel buildOrderViewModel(Models.OrderModel order, Enums.EnumOrderProcessingMethods nextMethod)
        {
            return new ViewModels.OrderViewModel() {
                CustomerUser = order.CustomerUser.ToUserDataViewModel(),
                StoreUser = order.CustomerUser.ToUserDataViewModel(),
                NextMethod = nextMethod
            };
        }

 

        private ViewModels.OrderViewModel buildOrderViewModel(string ErrorMessage)
        {
            return new ViewModels.OrderViewModel()
            {
               ErrorMessage = ErrorMessage
            };
        }

    }
}