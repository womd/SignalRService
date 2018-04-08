using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    public class OrderUtils
    {
        public static float GetToals(string orderId)
        {
            DAL.ServiceContext db = new DAL.ServiceContext();
            decimal sum = 0;
            var items = db.OrderItems.Where(ln => ln.Order.OrderIdentifier == orderId);
            foreach(var item in items)
            {
                sum = sum + item.Price;
            }
            return float.Parse(sum.ToString());
        }
    }
}