using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class MiningRoomViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DescriptionMarkdown { get; set; }
        public decimal HashesTotal { get; set; }
        public decimal XMR_Mined { get; set; }
        public decimal XMR_Needed { get; set; }
        public DateTime DataSnapshot { get; set; }
        public float HpsRoom { get; set; }
        public bool ShowControls { get; set; }
    }
}