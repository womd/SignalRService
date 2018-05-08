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

        public bool RemoveWinningRule(int ruleId)
        {
            var dbrm =_db.LuckyGameWinningRules.FirstOrDefault(ln => ln.ID == ruleId);
            if(dbrm != null)
            {
                _db.LuckyGameWinningRules.Remove(dbrm);
                _db.SaveChanges();
                return true;
            }
           
            return false;
           
        }

        public int GetOwnerIdForGroup(string group)
        {
            var service = _db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == group);
            if (service == null)
                return -1;

            return service.Owner.ID;

        }

        public bool AddOrUpdateWinningRule(ViewModels.LuckyGameWinningRuleViewModel rule, string group)
        {
            var service = _db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == group);
            if (service != null)
            {
                var game = service.LuckyGameSettings.FirstOrDefault();
                if(game != null)
                {
                    if(rule.Id == 0)
                    {
                        game.WinningRules.Add(new Models.LuckyGameWinningRule()
                        {
                            AmountMatchingCards = rule.AmountMatchingCards,
                            WinFactor = rule.WinFactor
                        });
                    }
                    else
                    {
                        var dbrule = game.WinningRules.FirstOrDefault(ln => ln.ID == rule.Id);
                        if (dbrule == null)
                            return false;

                        dbrule.AmountMatchingCards = rule.AmountMatchingCards;
                        dbrule.WinFactor = rule.WinFactor;
                    }

                    _db.SaveChanges();
                    return true;
                }
            }

            return false;
        }

    }
}