using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Enums
{
    public enum EnumOrderState
    {
        Undef = 0,
        ClientRequestOrderPlacement = 1,
        ClientPlacedOrder = 2,
        HostConfirmedOrder = 3,
        ClientOrderFinished = 4,
        ServerOrderFinished = 5,
        Cancel = 6,
        Error = 7
    }
}