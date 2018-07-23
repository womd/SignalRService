using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    static public class JSECoinMiningRoomInfoCache
    {
        private static Dictionary<int, ViewModels.MiningRoomJSECoinViewModel> VmCache;

        public static ViewModels.MiningRoomJSECoinViewModel GetItem(int MiningRoomId, int cacheTimeSec)
        {
            if (VmCache == null)
                VmCache = new Dictionary<int, ViewModels.MiningRoomJSECoinViewModel>();

            ViewModels.MiningRoomJSECoinViewModel result = null;
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

        public static void AddItem(int MiningRoomId, ViewModels.MiningRoomJSECoinViewModel vm)
        {
            if (VmCache == null)
                VmCache = new Dictionary<int, ViewModels.MiningRoomJSECoinViewModel>();

            vm.DataSnapshot = DateTime.Now;

            ViewModels.MiningRoomJSECoinViewModel fromCache = null;
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