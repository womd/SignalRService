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

        public void UpdateOrderState(string orderIdentifier, Enums.EnumOrderState state)
        {
            var dbOrder =_db.Orders.FirstOrDefault(ln => ln.OrderIdentifier == orderIdentifier);
            dbOrder.OrderState = state;
            _db.SaveChanges();
        }
    }
}