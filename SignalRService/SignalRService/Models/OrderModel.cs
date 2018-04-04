using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class OrderModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public virtual UserDataModel CustomerUser { get; set; }
        public virtual UserDataModel StoreUser { get; set; }
        public string OrderIdentifier { get; set; }
        public Enums.EnumOrderState OrderState { get; set; }
        public Enums.EnumOrderType OrderType { get; set; }
        public virtual ICollection<OrderItemModel>Items { get; set; }
        public Enums.EnumPaymentState PaymentState { get; set; }
        public Enums.EnumShippingState ShippingState { get; set; }
    }
}