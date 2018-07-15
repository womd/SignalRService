using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Interfaces;

namespace SignalRService.Factories
{
    public class MiningRoomFactory
    {

        public static IMiningRoom GetImplementation(Enums.EnumMiningRoomType miningRoomType)
        {
            switch(miningRoomType)
            {
                case Enums.EnumMiningRoomType.CoinIMP:
                    return new Implementation.MiningRoomBasic();
                case Enums.EnumMiningRoomType.JSECoin:
                    return new Implementation.MiningRoomCSECoin();
                
                default:
                    throw new NotImplementedException("no such implementation for Minigroom: " + miningRoomType);
                
            }

        }
    }
}