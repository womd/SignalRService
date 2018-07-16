using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRService.Interfaces
{
    public interface IMiningRoom
    {
        /// <summary>
        /// returns MiningRoomOverViewViewModel containing detals about the room
        /// </summary>
        dynamic GetOverview(int MiningRoomId);

      
        void SendRoomInfoUpdateToClients(dynamic vm, string signalRGroup);
        void SendRoomInfoUpdateToClient(dynamic vm, string connectionId);

        DTOs.GeneralHubResponseObject ProcessIncoming(DTOs.GeneralHubRequestObject Request);
    }
}
