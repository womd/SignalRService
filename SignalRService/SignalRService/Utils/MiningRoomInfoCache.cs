using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    static public class MiningRoomInfoCache
    {
        private static Dictionary<int, ViewModels.MiningRoomViewModel> VmCache;

        public static ViewModels.MiningRoomViewModel GetItem(int MiningRoomId, int cacheTimeSec)
        {
            if (VmCache == null)
                VmCache = new Dictionary<int, ViewModels.MiningRoomViewModel>();

            ViewModels.MiningRoomViewModel result = null;
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

        public static void AddItem(int MiningRoomId, ViewModels.MiningRoomViewModel vm)
        {
            if (VmCache == null)
                VmCache = new Dictionary<int, ViewModels.MiningRoomViewModel>();

            ViewModels.MiningRoomViewModel fromCache = null;
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