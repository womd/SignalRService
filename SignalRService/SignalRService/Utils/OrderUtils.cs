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

        public static MvcHtmlString Get_SortSelector_Orders()
        {
            string res = @"
            <div class=\'ui-field-contain\'>
            <label for='sort-select-orders'>" + BaseResource.Get("SortSelect") + @"} </label>
            <select name='sort-select-orders' id='sort-select-orders' multiple='multiple' data-native-menu='false'>
                <option>" + BaseResource.Get("SortSelectChoose") + @"</option>
                <option value='CreationDate ASC'>CreationDate ASC</option>
                <option value='CreationDate DSC'>CreationDate DSC</option>
                <option value='CustomerUser ASC'>CustomerUser ASC</option>
                <option value='CustomerUser DSC'>CustomerUser DSC</option>
                <option value='StoreUser ASC'>StoreUser ASC</option>
                <option value='StoreUser DSC'>StoreUser DSC</option>
                <option value='OrderState ASC'>OrderState ASC</option>
                <option value='OrderState DSC'>OrderState DSC</option>
                <option value='ShippingState ASC'>ShippingState ASC</option>
                <option value='ShippingState DSC'>ShippingState DSC</option>
                <option value='PaymentState ASC'>PaymentState ASC</option>
                <option value='PaymentState DSC'>PaymentState DSC</option>
            </select>
            </div>";

            res += @"<script>
                $('#sort-select-orders').bind( 'change', function(event, ui) {
                        OrdersSortSelectionChanged(event,ui);
                });</script>";

            return new MvcHtmlString(res);
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