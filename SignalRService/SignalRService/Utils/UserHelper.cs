using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    public class UserHelper
    {
        public static List<ViewModels.UserDataViewModel>getUsersList()
        {
            DAL.ServiceContext db = new DAL.ServiceContext();
            List<ViewModels.UserDataViewModel> users = new List<ViewModels.UserDataViewModel>();

            foreach(var item in db.UserData)
            {
                users.Add(item.ToUserDataViewModel());
            }
            return users;
        }
    }
}