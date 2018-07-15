using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SignalRService.Enums
{
    public enum EnumMiningRoomType
    {
        [Display(Name = "CoinIMPMiningRoom")]
        CoinIMP = 0,
        [Display(Name = "JSEMiningRoom")]
        JSECoin = 1
        
    }
}