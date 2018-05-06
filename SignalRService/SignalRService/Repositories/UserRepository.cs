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

        public double WithdrawMoney(int UserId, double amount)
        {
            return userContext.withDrawMoney(UserId, amount);
        }

        public double DepositMoneyToUser(int UserId, double amount)
        {
            return userContext.DepositMoneyToUser(UserId, amount);
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