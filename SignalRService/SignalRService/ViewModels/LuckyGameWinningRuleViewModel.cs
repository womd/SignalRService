using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class LuckyGameWinningRuleViewModel
    {
        public int Id { get; set; }
        public int AmountMatchingCards { get; set; }
        public float WinFactor { get; set; }
    }
}