using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRService.Interfaces
{
    public interface IOrderProcess
    {
        Enums.EnumOrderProcessingMethods GetNexProcess(Enums.EnumOrderState currentState);
       
    }
}
