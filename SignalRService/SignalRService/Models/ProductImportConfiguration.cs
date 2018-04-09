using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class ProductImportConfigurationModel
    {
        public int Id { get; set; }
        [Required]
        public virtual UserDataModel Owner { get; set; }

        [Required]
        public Enums.EnumImportType Type { get; set; }

        [Required]
        public string Source { get; set; }

        [Required]
        public string Name { get; set; }
    }
}