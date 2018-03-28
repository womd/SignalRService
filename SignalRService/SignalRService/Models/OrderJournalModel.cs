using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class OrderJournalModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public virtual OrderModel Order { get; set; }
        public Enums.EnumOrderState OrderState { get; set; }
        public virtual UserDataModel CustomerUser { get; set; }
        public virtual UserDataModel StoreUser { get; set; }

    }
}