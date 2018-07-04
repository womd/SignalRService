using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    public class ScriptDonater
    {
        private static ScriptDonater instance;
        private static readonly object lockobj = new object();
        private Dictionary<string, int> stats;
        private DAL.ServiceContext db;

        public ScriptDonater()
        {
            db = new DAL.ServiceContext();
            stats = new Dictionary<string, int>();
        }

        public static ScriptDonater Instance
        {
            get
            {
                //doublecheck locking   
                if (instance == null)
                {
                    lock (lockobj)
                    {
                        if (instance == null)
                        {
                            instance = new ScriptDonater();
                        }
                    }
                }
                return instance;
            }
        }

        public void AddStat(string ClientId)
        {
            if (!instance.stats.Any(ln => ln.Key == ClientId))
                instance.stats.Add(ClientId, 1);
            else
                instance.stats[ClientId]++;
        }

        public bool ShallDonate(string ClientId)
        {
            if (!instance.stats.Any(ln => ln.Key == ClientId))
                return false;

            int cntr = instance.stats[ClientId];
            int donateConnCntValue = (int) GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ScriptDonaterConnCntValue);
            if (cntr == donateConnCntValue)
            {
                return true;
            }
            return false;
        }

        public string Donate(string ClientId)
        {
            string donateClientId = (string) GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.ScriptDonaterClientId);
            instance.stats.Remove(ClientId);
            return donateClientId;
        }
    }
}