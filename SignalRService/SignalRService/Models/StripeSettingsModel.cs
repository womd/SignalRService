using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class StripeSettingsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }

       
        public virtual ServiceSettingModel Service { get; set; }

    }
}