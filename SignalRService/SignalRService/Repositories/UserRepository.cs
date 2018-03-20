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

        public ViewModels.UserDataViewModel GetUserFromSignalR(string connectionId)
        {
            var user = userContext.GetUserFromSignalRConnectionId(connectionId);
            return user.ToUserDataViewModel();
        }

        public ViewModels.UserDataViewModel GetUser(int Id)
        {
            return userContext.GetUser(Id).ToUserDataViewModel() ;
        }
    }

   
}