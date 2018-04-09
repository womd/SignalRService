using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class ProductImportModel
    {
        public int Id { get; set; }

        [Required]
        public virtual UserDataModel Owner { get; set; }

        public string SrcId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string GoogleProductCategory { get; set; }
        public string ProductType { get; set; }
        public string Link { get; set; }
        public string ImageLink { get; set; }
        public string PriceString { get; set; }
        public string Condition { get; set; }
        public string Availabiliby { get; set; }
        public string Gtin { get; set; }
        public string Mpn { get; set; }
        public string Brand { get; set; }
        public string CustomLabel0 { get; set; }
        public string CustomLabel1 { get; set; }
        public string CustomLabel2 { get; set; }
        public string CustomLabel3 { get; set; }
        public string CustomLabel4 { get; set; }
        public string gGuid { get; set; }
        public bool IdentifierExists { get; set; }

        public string ShippingCountry { get; set; }
        public string ShippingService { get; set; }
        public string ShippingPrice { get; set; }
    }
}