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
        ViewModels.MiningRoomViewModel GetOverview(int MiningRoomId);

        ViewModels.MiningRoomUpdateResult UpdateDescription(int MiningRoomId, string Content);
    }
}
