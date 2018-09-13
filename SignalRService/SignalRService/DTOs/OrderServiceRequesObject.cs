using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.DTOs
{
    public class OrderServiceRequesObject
    {
        public string Command { get; set; }
        public object CommandData { get; set; }

    }
}