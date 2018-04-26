using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    public static class HubExtensions
    {
        public static string GetRefererUrl(this IRequest Request)
        {
            var htc = Request.GetHttpContext();
            var vars = htc.Request.ServerVariables;
            string referer = vars["HTTP_REFERER"];
            return Request.GetHttpContext().Request.ServerVariables["HTTP_REFERER"];
        }

        public static string GetClientIp(this IRequest Request)
        {
            var httpcontext = Request.GetHttpContext();
            var forwardedFor = httpcontext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            var userIpAddress = String.IsNullOrWhiteSpace(forwardedFor) ?
                httpcontext.Request.ServerVariables["REMOTE_ADDR"] : forwardedFor.Split(',').Select(s => s.Trim()).First();
            return userIpAddress;
        }

    }
}