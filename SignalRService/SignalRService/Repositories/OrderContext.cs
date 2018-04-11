using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{

    public class OrderContext
    {
        private readonly DAL.ServiceContext _db;

        public OrderContext(DAL.ServiceContext db)
        {
            _db = db;
        }

        public Models.OrderModel GetOrder(int id)
        {
            return _db.Orders.FirstOrDefault(ln => ln.ID == id);
        }

        public Models.OrderModel GetOrder(string ident)
        {
            return _db.Orders.FirstOrDefault(ln => ln.OrderIdentifier == ident);
        }

        public Models.OrderModel AddOrUpdateOrder(Models.OrderModel model)
        {
            Models.OrderModel dbmodel;
            if (model.ID == 0)
            {
                dbmodel =_db.Orders.Add(model);
            }
            else
            {
                dbmodel = _db.Orders.FirstOrDefault(ln => ln.ID == model.ID);
                dbmodel.CustomerUser = model.CustomerUser;
                dbmodel.StoreUser = model.StoreUser;
                dbmodel.OrderIdentifier = model.OrderIdentifier;
                dbmodel.OrderState = model.OrderState;
                dbmodel.OrderType = model.OrderType;
                dbmodel.Items = model.Items;
            }
            _db.SaveChanges();
            return dbmodel;
        }

        public List<Models.OrderModel> GetClientOrders(int UserId )
        {
            return _db.Orders.Where(ln => ln.CustomerUser.ID == UserId).ToList();
        }

        public List<Models.OrderModel> GetHostOrders(int UserId)
        {
            return _db.Orders.Where(ln => ln.StoreUser.ID == UserId).ToList();
        }

        public void UpdateOrderState(string orderIdentifier, Enums.EnumOrderState state)
        {
            var dbOrder =_db.Orders.FirstOrDefault(ln => ln.OrderIdentifier == orderIdentifier);
            dbOrder.OrderState = state;
            _db.SaveChanges();
        }

        public void UpdateShippingState(string orderIdentifier, Enums.EnumShippingState shippingState)
        {
            var dbOrder = _db.Orders.FirstOrDefault(ln => ln.OrderIdentifier == orderIdentifier);
            dbOrder.ShippingState = shippingState;
            _db.SaveChanges();
        }

        public void UpdatePaymentState(string orderIdentifier, Enums.EnumPaymentState paymentState)
        {
            var dbOrder = _db.Orders.FirstOrDefault(ln => ln.OrderIdentifier == orderIdentifier);
            dbOrder.PaymentState = paymentState;
            _db.SaveChanges();
        }

        public List<Models.OrderModel>GetOrders()
        {
            return _db.Orders.ToList();
        }

        /// <summary>
        /// get Orders for paged / sorted jTable
        /// </summary>
        /// <param name="StartIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="sorting"></param>
        /// <returns></returns>
        public List<Models.OrderModel>GetOrders(int StartIndex, int PageSize, string sorting)
        {
            var orders = new List<Models.OrderModel>();
            bool executed = false;
            if (sorting.IndexOf("StoreUser") != -1)
            {
                if(sorting.IndexOf("ASC") != -1)
                {
                    orders = _db.Orders.OrderBy(ln => ln.StoreUser.IdentityName).Skip(StartIndex).Take(PageSize).ToList();
                }
                else
                {
                    orders = _db.Orders.OrderByDescending(ln => ln.StoreUser.IdentityName).Skip(StartIndex).Take(PageSize).ToList();
                }
                
                executed = true;
            }
            if (sorting.IndexOf("CustomerUser") != -1)
            {
                if (sorting.IndexOf("ASC") != -1)
                {
                    orders = _db.Orders.OrderBy(ln => ln.CustomerUser.IdentityName).Skip(StartIndex).Take(PageSize).ToList();
                }
                else
                {
                    orders = _db.Orders.OrderByDescending(ln => ln.CustomerUser.IdentityName).Skip(StartIndex).Take(PageSize).ToList();
                }
                executed = true;
            }

            if (!executed)
            {
                string query = @"select * from OrderModels o order by " + sorting + @"
                            offset " + StartIndex + @" rows 
                            FETCH NEXT " + PageSize + " rows only";

                orders = _db.Orders.SqlQuery(query).ToList();
            }
            return orders;
           // var baseq = _db.Orders.Skip(StartIndex).Take(PageSize).OrderBy(ln => ln.CreationDate);
        }

        /// <summary>
        /// removes an order and its items
        /// </summary>
        /// <param name="orderIdentifier"></param>
        public void Remove(string orderIdentifier)
        {
            var toRemove = _db.Orders.FirstOrDefault(x => x.OrderIdentifier == orderIdentifier);

            _db.OrderItems.RemoveRange(toRemove.Items);
            _db.Orders.Remove(toRemove);

            _db.SaveChanges();
        }
    }
}