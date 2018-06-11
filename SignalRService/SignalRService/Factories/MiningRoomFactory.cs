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
                case Enums.EnumMiningRoomType.Basic:
                    return new Implementation.MiningRoomBasic();
                default:
                    throw new NotImplementedException("no such implementation for Minigroom: " + miningRoomType);
                
            }

        }
    }
}