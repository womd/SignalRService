using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Interfaces;

namespace SignalRService.Factories
{
    public class OrderProcessFactory
    {

        public static IOrderProcess GetOrderProcessImplementation(Enums.EnumOrderType type)
        {
            switch(type)
            {
                case Enums.EnumOrderType.Default:
                    return new Implementation.OrderProcessDefault();
                case Enums.EnumOrderType.Drone:
                    return new Implementation.OrderProcessDrone();
                default:
                    return null;
            }

        }
    }
}