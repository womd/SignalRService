using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Interfaces;

namespace SignalRService.Implementation
{
    public class OrderProcessDefault : IOrderProcess
    {
        public Enums.EnumOrderProcessingMethods GetNexProcess(Enums.EnumOrderState currentState)
        {
            switch (currentState)
            {
                case Enums.EnumOrderState.Undef:
                    return Enums.EnumOrderProcessingMethods.Undefined;
                case Enums.EnumOrderState.ClientRequestOrderPlacement:
                    return Enums.EnumOrderProcessingMethods.NewOrder;
                default:
                    return Enums.EnumOrderProcessingMethods.Undefined;
            }
        }
    }
}