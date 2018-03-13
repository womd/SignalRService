using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Enums
{
    public enum EnumOrderState
    {
        Pending= 0,
        Confirmed = 1,
        Finished = 2,
        Refund = 3,
        Cancel = 4
    }
}