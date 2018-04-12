using SignalRService.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

      

        public static List<ViewModels.OrderViewModel>SortByConfig(List<ViewModels.OrderViewModel> input, List<Hubs.UiSorter> sorters)
        {
            foreach(var sorter in sorters)
            {
                switch (sorter.Expression)
                {
                    case "CreationDate ASC":
                        return input.OrderBy(ln => ln.CreationDate).ToList();
                    case "CreationDate DSC":
                        return input.OrderByDescending(ln => ln.CreationDate).ToList();
                    case "CustomerUser ASC":
                        return input.OrderBy(ln => ln.CustomerUser.Name).ToList();
                    case "CustomerUser DSC":
                        return input.OrderByDescending(ln => ln.CustomerUser.Name).ToList();
                    case "StoreUser ASC":
                        return input.OrderBy(ln => ln.StoreUser.Name).ToList();
                    case "StoreUser DSC":
                        return input.OrderByDescending(ln => ln.StoreUser.Name).ToList();
                    case "OrderState ASC":
                        return input.OrderBy(ln => ln.OrderState).ToList();
                    case "OrderState DSC":
                        return input.OrderByDescending(ln => ln.OrderState).ToList();
                    case "ShippingState ASC":
                        return input.OrderBy(ln => ln.ShippingState).ToList();
                    case "ShippingState DSC":
                        return input.OrderByDescending(ln => ln.ShippingState).ToList();
                    case "PaymentState ASC":
                        return input.OrderBy(ln => ln.PaymentState).ToList();
                    case "PaymentState DSC":
                        return input.OrderByDescending(ln => ln.PaymentState).ToList();
                    default:
                        return input.OrderByDescending(x => x.CreationDate).ToList();
                }
            }
            return input;
        }
    }
}