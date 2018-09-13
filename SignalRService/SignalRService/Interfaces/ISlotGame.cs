using SignalRService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Interfaces
{
    public interface ISlotGame
    {
        GeneralHubResponseObject ProcessIncoming(System.Security.Principal.IPrincipal User, GeneralHubRequestObject Request);
    }
}