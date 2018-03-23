using SignalRService.Hubs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class UserDataModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        
        public string IdentityName { get; set; }

        public virtual ICollection<SignalRConnectionModel> SignalRConnections { get; set; }
        public virtual ICollection<ProductModel> Products { get; set; }
        public virtual ICollection<ServiceSettingModel> ServiceSettings { get; set; }
    }
}