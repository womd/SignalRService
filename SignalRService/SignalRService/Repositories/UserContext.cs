using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Repositories
{
    public class UserContext
    {
        private readonly DAL.ServiceContext _db;

        public UserContext(DAL.ServiceContext db)
        {
            _db = db;
        }

        public double getTotalMoney(int UserId)
        {
            var useerdata =_db.UserData.FirstOrDefault(ln => ln.ID == UserId);
            if (useerdata != null)
                return useerdata.TotalMoney;

            return 0;
        }

        public double withDrawMoney(int UserId, double Amount)
        {
            var userdata = _db.UserData.FirstOrDefault(ln => ln.ID == UserId);
            if(userdata != null)
            {
                userdata.TotalMoney = userdata.TotalMoney - Amount;
                _db.SaveChanges();
            }
            return userdata.TotalMoney;
        }

        public double DepositMoneyToUser(int UserId, double Amount)
        {
            var userdata = _db.UserData.FirstOrDefault(ln => ln.ID == UserId);
            if(userdata != null) {
                userdata.TotalMoney = userdata.TotalMoney + Amount;
                _db.SaveChanges();
            }
            return userdata.TotalMoney;
        }

        public Models.UserDataModel GetUser(string identifier)
        {
            return _db.UserData.FirstOrDefault(ln => ln.IdentityName == identifier);
        }

        public Models.UserDataModel GetUserFromSignalRConnectionId(string connectionId)
        {
            var dbConn =_db.SignalRConnections.FirstOrDefault(ln => ln.SignalRConnectionId == connectionId);
            return dbConn.User;
        }

   

        public Models.UserDataModel GetUser(int Id)
        {
            return _db.UserData.FirstOrDefault(ln => ln.ID == Id);
        }

     
   
    }
}