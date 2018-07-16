using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class JSECoinMinerConfigurationModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ClientId { get; set; }
        public string SiteId { get; set; }
        public string SubId { get; set; }
        public virtual ICollection<ServiceSettingModel> ServiceSetting { get; set; }
    }
}