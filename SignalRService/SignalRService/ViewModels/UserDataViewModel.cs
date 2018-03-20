using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class UserDataViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string>SignalRConnections { get; set; }
    }
}