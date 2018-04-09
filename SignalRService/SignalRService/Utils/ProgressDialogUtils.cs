using Microsoft.AspNet.SignalR;
using SignalRService.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    public class ProgressDialogUtils
    {
        public static void Show(string id, string title, string message, int pgvalue, List<string> userconnectionIds)
        {
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Clients(userconnectionIds).showProgressDialog(id, title, message, pgvalue);
        }

        public static void Update(string id, string message, int pgvalue, List<string> userconnectionIds)
        {
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Clients(userconnectionIds).updateProgressDialog(id, message, pgvalue);
        }

        public static void Close(string id, List<string> userconnectionIds)
        {
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Clients(userconnectionIds).closeProgressDialog(id);
        }
    }
}