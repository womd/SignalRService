using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Enums
{
    public enum EnumOrderState
    {
        Undef = 0,
        ClientPlacedOrder = 1,
        HostConfirmedOrder = 2,
        ClientOrderFinished = 3,
        ServerOrderFinished = 4,
        Cancel = 6,
        Error = 7
    }
}