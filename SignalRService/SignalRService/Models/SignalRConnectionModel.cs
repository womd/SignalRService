using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class SignalRConnectionModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string SignalRConnectionId { get; set; }

        public Enums.EnumSignalRConnectionState ConnectionState { get; set; }
        public string RefererUrl { get; set; }
        public string RemoteIp { get; set; }
       
        public virtual UserDataModel User { get; set; }

        public virtual MinerStatusModel MinerStatus { get; set; }
       
        public virtual ICollection<SignalRGroupsModel>Groups { get; set; }
    }
}