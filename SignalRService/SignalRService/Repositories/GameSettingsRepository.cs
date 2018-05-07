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
            Utils.LuckyGameUtils.AvailableMoneyUpdate(amount, group);
            return res;
        }

        public double WidthdrawMoneyFromGame(double amount, string group)
        {
            var res = context.WidtdrawMoneyFromGame(amount, group);
            Utils.LuckyGameUtils.AvailableMoneyUpdate(amount, group);
            return res;
        }
    }
}