using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.HubPipelineModules
{
    public class GroupMonitorPipelineModule : HubPipelineModule
    {
        protected override bool OnBeforeIncoming(IHubIncomingInvokerContext context)
        {
            if(Enum.TryParse(context.MethodDescriptor.Name, out Enums.EnumServiceHubMethods res))
            {
                switch (res)
                {
                    case Enums.EnumServiceHubMethods.JoinGroup:
                        DAL.SignalRConnections.Instance.GroupAddOrUpdate(context.Hub.Context.ConnectionId, context.Args.ToString());
                        break;
                    case Enums.EnumServiceHubMethods.LeaveGroup:
                        DAL.SignalRConnections.Instance.GroupRemove(context.Hub.Context.ConnectionId, context.Args.ToString());
                        break;
                    default:
                        break;
                }
            }
            return true;
          
        }
        protected override bool OnBeforeOutgoing(IHubOutgoingInvokerContext context)
        {
            return true;
        }
    }
}