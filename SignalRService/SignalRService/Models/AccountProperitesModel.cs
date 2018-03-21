using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class AccountPropertiesModel : BaseModel
    {
        [Key]
        public int ID { get; set; }
        public string UserId {get;set;} 
        public virtual ICollection<ServiceSettingModel> ServiceSettings { get; set; }
    }
}