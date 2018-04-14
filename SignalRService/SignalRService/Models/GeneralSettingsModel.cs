using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class GeneralSettingsModel : BaseModel
    {
        public int Id { get; set; }
        [Required]
        public Enums.EnumSettingType Type { get; set; }
        [Index(IsUnique = true)]
        public Enums.EnumGeneralSetting GeneralSetting { get; set; }
        [Required]
        public string Value { get; set; }
    }
}