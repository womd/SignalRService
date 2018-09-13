using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Interfaces;

namespace SignalRService.Factories
{
    public class SlotGameFactory
    {

        public static ISlotGame GetImplementation(Enums.EnumSlotGame gameType)
        {
            switch(gameType)
            {
               
                case Enums.EnumSlotGame.LuckyGame:
                    return new Implementation.SlotGameDefault();
                
                default:
                    throw new NotImplementedException("no such implementation for Minigroom: " + gameType);
                
            }

        }
    }
}