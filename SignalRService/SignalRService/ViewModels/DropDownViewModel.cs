using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.ViewModels
{
    public class DropDownViewModel
    {
        public string SelectedItemId { get; set; }
        public List<SelectListItem> Items { get; set; }
    }
}