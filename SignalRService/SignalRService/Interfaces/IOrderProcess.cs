using SignalRService.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRService.Interfaces
{
    public interface IOrderProcess
    {
        /// <summary>
        /// check if orderdata is valid for a new order
        /// </summary>
        /// <param name="orderDto"></param>
        /// <returns>OrderViewModel - on Error - ErrorMessage is set</returns>
        ViewModels.OrderViewModel CheckOrder(OrderDataDTO orderDto);

      
        ViewModels.OrderViewModel ProcessOrder(ViewModels.OrderViewModel order);
        
    }
}
