using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SignalRService.Models;

namespace SignalRService.Models
{
    public class MiningRoomModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("ServiceSetting")]
        public int ServiceSettingId { get; set; }
        public virtual ServiceSettingModel ServiceSetting { get; set; }
    }
}