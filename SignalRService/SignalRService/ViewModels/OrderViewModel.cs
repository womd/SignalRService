using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class OrderViewModel
    {
        public UserDataViewModel CustomerUser { get; set; }
        public UserDataViewModel StoreUser { get; set; }
        public string OrderIdentifier { get; set; }
        public Enums.EnumOrderState OrderState { get; set; }
        public Enums.EnumOrderType OrderType { get; set; }
        public string ErrorMessage {get;set;}
        public Enums.EnumOrderProcessingMethods NextMethod { get; set; }
        public List<ViewModels.OrderItemViewModel> Items { get; set; }
        public DateTime CreationDate { get; set; }
        public Enums.EnumPaymentState PaymentState { get; set; }
        public Enums.EnumShippingState ShippingState { get; set; }

    }

    
}