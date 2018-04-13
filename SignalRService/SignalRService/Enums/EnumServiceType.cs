using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SignalRService.Enums
{
    public enum EnumServiceType
    {
        [Display(Name = "ServiceTypeOrderService")]
        OrderService = 0,
        [Display(Name = "ServiceTypeTaxiService")]
        TaxiService = 1,
        [Display(Name = "ServiceTypeSecurityService")]
        SecurityService = 2,
        [Display(Name = "ServiceTypeOrderServiceDrone")]
        OrderServiceDrone = 3
    }
}