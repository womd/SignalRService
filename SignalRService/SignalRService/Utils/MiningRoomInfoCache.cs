using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    static public class MiningRoomInfoCache
    {
        private static Dictionary<int, ViewModels.MiningRoomCoinIMPViewModel> VmCache;

        public static ViewModels.MiningRoomCoinIMPViewModel GetItem(int MiningRoomId, int cacheTimeSec)
        {
            if (VmCache == null)
                VmCache = new Dictionary<int, ViewModels.MiningRoomCoinIMPViewModel>();

            ViewModels.MiningRoomCoinIMPViewModel result = null;
            if (VmCache.TryGetValue(MiningRoomId, out result))
            {
                var refDate = result.DataSnapshot.AddSeconds(cacheTimeSec);
                if (refDate > DateTime.Now)
                    return result;
                else
                    VmCache.Remove(MiningRoomId);
            }
            return null;
        }

        public static void AddItem(int MiningRoomId, ViewModels.MiningRoomCoinIMPViewModel vm)
        {
            if (VmCache == null)
                VmCache = new Dictionary<int, ViewModels.MiningRoomCoinIMPViewModel>();

            ViewModels.MiningRoomCoinIMPViewModel fromCache = null;
            if( VmCache.TryGetValue(MiningRoomId, out fromCache) )
            {
                VmCache[MiningRoomId] = vm;
            }
            else
            {
                VmCache.Add(MiningRoomId, vm);
            }
           
        }

    }
}