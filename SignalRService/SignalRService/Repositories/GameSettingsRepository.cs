using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{
    public class GameSettingsRepository
    {
        private Repositories.GameSettingsContext context;

        public GameSettingsRepository(DAL.ServiceContext _db)
        {
            context = new GameSettingsContext(_db);
        }

        public double AddMoneyToGame(double amount, string group)
        {
            var res = context.AddMoneyToGame(amount, group);
            Utils.LuckyGameUtils.AvailableMoneyUpdate(res, group);
            return res;
        }

        public double WidthdrawMoneyFromGame(double amount, string group)
        {
            var res = context.WidtdrawMoneyFromGame(amount, group);
            Utils.LuckyGameUtils.AvailableMoneyUpdate(amount, group);
            return res;
        }

        public bool RemoveWinningRule(int ruleId, string group)
        {
            var res = context.RemoveWinningRule(ruleId);
            if (res)
            {
                Utils.LuckyGameUtils.WinningRulesUpdate(group);
                return true;
            }
            return false;
        }

        public int GetOwnerIdForGroup(string group)
        {
            return context.GetOwnerIdForGroup(group);
        }

        public bool AddOrUpdateWinningRule(ViewModels.LuckyGameWinningRuleViewModel rule, string group)
        {
            var res = context.AddOrUpdateWinningRule(rule, group);
            if (res)
            {
                Utils.LuckyGameUtils.WinningRulesUpdate(group);
                return true;
            }
            return false;
        }
    }
}