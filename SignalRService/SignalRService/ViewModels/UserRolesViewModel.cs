using SignalRService.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class UserRolesViewModel
    {
      public string IdentityId { get; set; }
      public string IdentityName { get; set; }
      public List<ViewModels.RoleViewModel> Roles { get; set; }
    }
}