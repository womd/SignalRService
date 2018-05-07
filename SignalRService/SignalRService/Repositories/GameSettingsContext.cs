using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{
    public class GameSettingsContext
    {
        private readonly DAL.ServiceContext _db;
        public GameSettingsContext(DAL.ServiceContext db)
        {
            _db = db;
        }

        public double AddMoneyToGame(double amount, string group)
        {
            double res = 0;
            var gs =_db.LuckyGameSettings.FirstOrDefault(ln => ln.ServiceSettings.ServiceUrl == group);
            if (gs != null)
            {
                gs.MoneyAvailable = gs.MoneyAvailable + amount;
                res = gs.MoneyAvailable;
            }
            _db.SaveChanges();
            return res;
        }

        public double WidtdrawMoneyFromGame(double amount, string group)
        {
            double res = 0;
            var gs = _db.LuckyGameSettings.FirstOrDefault(ln => ln.ServiceSettings.ServiceUrl == group);
            if (gs != null)
            {
                gs.MoneyAvailable = gs.MoneyAvailable - amount;
                res = gs.MoneyAvailable;
            }
            _db.SaveChanges();
            return res;
        }

    }
}