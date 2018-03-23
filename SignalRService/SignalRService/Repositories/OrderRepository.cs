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

        public ViewModels.OrderItemViewModel CheckProduct(Hubs.OrderItem itemDTO )
        {
            ViewModels.OrderItemViewModel vm = new ViewModels.OrderItemViewModel();
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

            vm.Amount = itemDTO.Amount;

            return vm;
        }

        public List<ViewModels.OrderItemViewModel> CheckProducts(List<Hubs.OrderItem>items)
        {
            List<ViewModels.OrderItemViewModel> res = new List<ViewModels.OrderItemViewModel>();
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
     
    

        public ViewModels.OrderViewModel AddOrder(List<ViewModels.OrderItemViewModel>items, ViewModels.UserDataViewModel customerUser, ViewModels.UserDataViewModel storeUser)
        {
            var Order = new Models.OrderModel()
            {
                OrderState = Enums.EnumOrderState.ClientPlacedOrder,
                OrderType = Enums.EnumOrderType.Default,
                OrderIdentifier = Guid.NewGuid().ToString(),
                CustomerUser = userContext.GetUser(customerUser.Id),
                StoreUser = userContext.GetUser(storeUser.Id)
            };

            Order.Items = new List<Models.OrderItemModel>();
            foreach (var item in items)
            {
                Order.Items.Add(new Models.OrderItemModel()
                {
                    PartNo = item.PartNumber,
                    Price = item.Price,
                    Name = item.Name,
                    Amount = item.Amount,
                });
            }

            Order.OrderIdentifier = Guid.NewGuid().ToString();
            var newObj = orderContext.AddOrUpdateOrder(Order);
            return newObj.ToOrderViewModel();
        }

        public List<ViewModels.OrderViewModel>GetOrders(int userId, Enums.EnumGuiType guiType)
        {
            List<Models.OrderModel> dbOrders = new List<Models.OrderModel>();
            switch (guiType)
            {
                case Enums.EnumGuiType.Undef:
                    break;
                case Enums.EnumGuiType.Client:
                    dbOrders = orderContext.GetClientOrders(userId);
                    break;
                case Enums.EnumGuiType.Host:
                    dbOrders = orderContext.GetClientOrders(userId);
                    break;
                case Enums.EnumGuiType.Admin:
                    break;
                default:
                    break;
            }

            var reslist = new List<ViewModels.OrderViewModel>();
            foreach(var item in dbOrders)
            {
                reslist.Add(item.ToOrderViewModel());
            }

            return reslist;
        }

        public ViewModels.OrderViewModel GetOrder(string orderIdentifier)
        {
            return orderContext.GetOrder(orderIdentifier).ToOrderViewModel();
        }

        public void UpdateOrderState(string orderIdentifier, Enums.EnumOrderState state)
        {
            orderContext.UpdateOrderState(orderIdentifier, state);
        }

    }
}