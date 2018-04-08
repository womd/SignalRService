using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Interfaces;

namespace SignalRService.Factories
{
    public class OrderProcessFactory
    {

        public static IOrderProcess GetOrderProcessImplementation(Enums.EnumServiceType serviceType)
        {
            switch(serviceType)
            {
                case Enums.EnumServiceType.OrderService:
                    return new Implementation.OrderProcessDefault();
                case Enums.EnumServiceType.OrderServiceDrone:
                    return new Implementation.OrderProcessDrone();
                default:
                    return null;
            }

        }
    }
}