﻿using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Utils;

namespace SignalRService.HubPipelineModules
{
    public class GroupMonitorPipelineModule : HubPipelineModule
    {
      
         
        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            DAL.ServiceContext db = new DAL.ServiceContext();
            var dbCon = db.SignalRConnections.FirstOrDefault(ln => ln.SignalRConnectionId == context.Hub.Context.ConnectionId);
            if(dbCon == null)
            {
              //  SimpleLogger logger = new SimpleLogger();
              //  logger.Info("added connection on BeForeIncoming");
                db.AddConnection(context.Hub.Context.ConnectionId, context.Hub.Context.Request.GetRefererUrl(), context.Hub.Context.Request.GetClientIp());
            }
            
            //if (Enum.TryParse(context.MethodDescriptor.Name, out Enums.EnumServiceHubMethods res))
            //{
            //    var dbCon = db.SignalRConnections.FirstOrDefault(ln => ln.SignalRConnectionId == context.Hub.Context.ConnectionId);
            //    switch (res)
            //    {
            //        case Enums.EnumServiceHubMethods.JoinGroup:
            //            if (!dbCon.Groups.Contains(context.Args[0].ToString()))
            //                dbCon.Groups.Add(context.Args[0].ToString());

            //            break;
            //        case Enums.EnumServiceHubMethods.LeaveGroup:
            //            var rm = dbCon.Groups.Remove(context.Args[0].ToString());
            //            break;
            //        default:
            //            break;
            //    }
            //    db.SaveChanges();

            //}
            return true;
          
        }
        protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
        {
            return true;
        }
    }
}