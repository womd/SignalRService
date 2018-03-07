using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class UserDataViewModel
    {
        public string ConnectionId { get; set; }
        public string ConnectionState { get; set; }
    }
}