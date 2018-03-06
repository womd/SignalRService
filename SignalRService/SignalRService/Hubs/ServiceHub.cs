using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRService.Hubs
{
    public class ServiceHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}