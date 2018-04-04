using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class ServiceSettingModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual UserDataModel Owner { get; set; }

        public string ServiceName { get; set; }
        [Index("ServiceUrl_Index", IsUnique = true)]
        [MaxLength(16)]
        public string ServiceUrl { get; set; }
        public Enums.EnumServiceType ServiceType { get; set; }


        public virtual ICollection<StripeSettingsModel> StripeSettings { get; set; }
    }

   
}