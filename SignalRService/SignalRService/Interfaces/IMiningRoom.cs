using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRService.Interfaces
{
    interface IMiningRoom
    {
        /// <summary>
        /// returns MiningRoomOverViewViewModel containing detals about the room
        /// </summary>
        ViewModels.MiningRoomViewModel GetOverview(int MiningRoomId);

    }
}
