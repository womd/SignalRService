using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string PartNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }
        public int ErrorNumber { get; set; }
        public string ErrorMessage { get; set; }
        public decimal Price { get; set; }
    }
}