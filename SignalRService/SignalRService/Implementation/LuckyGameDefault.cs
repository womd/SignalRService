using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRService.Hubs;
using SignalRService.Interfaces;
using SignalRService.ViewModels;
using SignalRService.DTOs;
using Newtonsoft.Json;
using SignalRService.Localization;
using System.Globalization;
using System.Security.Principal;

namespace SignalRService.Implementation
{
    public class SlotGameDefault : ISlotGame
    {

        public GeneralHubResponseObject ProcessIncoming(System.Security.Principal.IPrincipal User, GeneralHubRequestObject Request)
        {
            GeneralHubResponseObject result = new GeneralHubResponseObject();
            if (!User.Identity.IsAuthenticated)
            {
                result.Success = false;
                result.ErrorMessage = SignalRService.Localization.BaseResource.Get("MsgLoginFirst");
                return result;
            }

            return result;
        }

    }
}