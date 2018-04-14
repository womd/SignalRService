using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class GeneralSettingsViewModel
    {
        public int Id { get; set; }
        public Enums.EnumSettingType Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}