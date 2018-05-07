using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.Utils;

namespace SignalRService.Repositories
{
    public class UserRepository
    {
        private Repositories.UserContext userContext;

        public UserRepository(DAL.ServiceContext db)
        {
            userContext = new UserContext(db);
        }

        public double GetUserTotalMoney(int UserId)
        {
            return userContext.getTotalMoney(UserId);
        }

        public double WithdrawMoneyFromUser(int UserId, double amount)
        {
            var res = userContext.withDrawMoney(UserId, amount);
            Utils.LuckyGameUtils.SendUserTotalMoneyUpdate(UserId, res);
            return res;
        }

        public double DepositMoneyToUser(int UserId, double amount)
        {
            var res = userContext.DepositMoneyToUser(UserId, amount);
            Utils.LuckyGameUtils.SendUserTotalMoneyUpdate(UserId,res);
            return res;
        }

        public ViewModels.UserDataViewModel GetUserFromSignalR(string connectionId)
        {
            var user = userContext.GetUserFromSignalRConnectionId(connectionId);
            return user.ToUserDataViewModel();
        }

        public ViewModels.UserDataViewModel GetUser(int Id)
        {
            return userContext.GetUser(Id).ToUserDataViewModel() ;
        }

        public ViewModels.UserDataViewModel GetUser(string IdentityName)
        {
            return userContext.GetUser(IdentityName).ToUserDataViewModel();
        }
    }

   
}