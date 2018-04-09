using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class ProductImportConfigurationViewModel
    {
        public int Id { get; set; }
        public ViewModels.UserDataViewModel Owner { get; set; }
        public Enums.EnumImportType Type { get; set; }
        public string Source { get; set; }
        public string Name { get; set; }
    }
}