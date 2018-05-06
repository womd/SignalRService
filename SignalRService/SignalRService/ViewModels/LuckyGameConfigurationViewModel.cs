using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.ViewModels
{
    public class LuckyGameConfigurationViewModel
    {
        public int Id { get; set; }
        public double MoneyAvailable { get; set; }
        public List<LuckyGameWinningRuleViewModel> WinningRules { get; set; }
        public string SignalRGroup { get; set; }
    }
}