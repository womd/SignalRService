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
        private List<string> _mGroups;

        public List<string> Groups
        {
            get
            {
                if (_mGroups == null)
                    _mGroups = new List<string>();
                return _mGroups;
            }
            set { _mGroups = value; }
        }

        public virtual UserDataModel User { get; set; }

        public virtual ICollection<MinerStatusModel> MinerStatus { get; set; }
       
    }
}