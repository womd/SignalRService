using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SignalRService.Models
{
    public class LuckyGameSettingsModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public double MoneyAvailable { get; set; }
        public virtual ICollection<LuckyGameWinningRule> WinningRules { get; set; }
        public virtual ServiceSettingModel ServiceSettings { get; set; }
    }

    public class LuckyGameWinningRule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int AmountMatchingCards { get; set; }
        public float WinFactor { get; set; }
    }
}