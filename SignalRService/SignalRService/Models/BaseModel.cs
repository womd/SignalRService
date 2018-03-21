using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class BaseModel
    {
        protected BaseModel()
        {
            CreationDate = DateTime.Now;
        }

        [ReadOnly(true)]
        public DateTime CreationDate { get; set; }
        public bool Archived { get; set; }
    }
}