using SignalRService.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Utils
{
    public static class JTableHelper
    {
        public static MvcHtmlString Get_FilterSelector_Products()
        {
            string res = @"
            <fieldset>
            <label for='filter-select-products'>" + BaseResource.Get("FilterSelect") + @"} </label>
            <select name='filter-select-products' id='filter-select-products'>
                <option>" + BaseResource.Get("SortSelectChoose") + @"</option>
                <option value='PartNumber'>PartNumber</option>
                <option value='ProductName'>ProductName</option>
                <option value='Description'>Description</option>
                <option value='Owner'>Owner</option>
                <option value='Price'>Price</option>
            </select>
            
                <label for='orderfilterinput'>" + BaseResource.Get("FilterInput") + @"</label>
                <input type='text' name='orderfilterinput' id='orderfilterinput'>
            ";

            res += @"<input type='button' class='ui-button ui-widget ui-corner-all' onclick='filtersearchbuttonclicked()' value='" + BaseResource.Get("BtnFilterSearch") + "'></input>";

            res += "</fieldset>";

            res += @"<script>
                    $(function() {
                       //  $('#filter-select-products').selectmenu();
                    });
                    </script>";
           

            return new MvcHtmlString(res);
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
    }
}