using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class CoinIMPMinerConfigurationModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ScriptUrl { get; set; }
        public string ClientId { get; set; }
        public float Throttle { get; set; }
        public int StartDelayMs { get; set; }
        public int ReportStatusIntervalMs { get; set; }
        public virtual ICollection<ServiceSettingModel> ServiceSetting { get; set; }
    }
}