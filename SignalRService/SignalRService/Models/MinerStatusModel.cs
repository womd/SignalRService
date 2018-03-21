using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class MinerStatusModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int SignalRConnectionID { get; set; }
        public virtual SignalRConnectionModel SignalRConnection { get; set; }  

        public bool Running { get; set; }
        public bool OnMobile { get; set; }
        public bool WasmEnabled { get; set; }
        public bool IsAutoThreads { get; set; }
        public float Hps { get; set; }
        public int Threads { get; set; }
        public float Throttle { get; set; }
        public int Hashes { get; set; }
    }
}