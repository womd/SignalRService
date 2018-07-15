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

      
        void SendRoomInfoUpdateToClients(ViewModels.MiningRoomCoinIMPViewModel vm, string signalRGroup);
        void SendRoomInfoUpdateToClient(ViewModels.MiningRoomCoinIMPViewModel vm, string connectionId);

        DTOs.GeneralHubResponseObject ProcessIncoming(DTOs.GeneralHubRequestObject Request);
    }
}
