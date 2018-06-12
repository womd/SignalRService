using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Models;

namespace SignalRService.Models
{
    public class SignalRGroupsModel
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public virtual ICollection<SignalRConnectionModel>Connections { get; set; }
    }
}