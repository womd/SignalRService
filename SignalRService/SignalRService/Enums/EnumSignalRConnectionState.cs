using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SignalRService.Enums
{
    public enum EnumSignalRConnectionState
    {
        //use data-annotations for naming, but don't want to use resourcefile, will implement resourcebuilder-like thing...

        //[Display(Name = "Connected")]
        Connected,
        //[Display(Name = "Connecting")]
        Connecting,
        //[Display(Name = "Disconnected")]
        Disconnected,
        //[Display(Name = "Reconnecting")]
        Reconnecting
    }
}