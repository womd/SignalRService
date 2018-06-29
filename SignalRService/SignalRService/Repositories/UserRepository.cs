using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

        public Models.UserDataModel GetDbUser(string IdentityName)
        {
            return userContext.GetUser(IdentityName);
        }

        public Models.UserDataModel GetDefaultUser()
        {
            return userContext.GetUser("Anonymous");
        }

        public string GetAspNetUsersXML()
        {
            var appDbContext = new Models.ApplicationDbContext();
           
            DTOs.AspNetUserDataDTO theTransferObject = new DTOs.AspNetUserDataDTO();
            theTransferObject.Users = new List<DTOs.AspNetUsersDTO>();
            theTransferObject.Roles = new List<DTOs.AspNetRolesDTO>();
            theTransferObject.UserLogins = new List<DTOs.AspNetUserLoginsDTO>();
            theTransferObject.UserRoles = new List<DTOs.AspNetUserRolesDTO>();

            var userManager = new UserManager<Models.ApplicationUser>(new UserStore<Models.ApplicationUser>(appDbContext));
            foreach (var item in appDbContext.Users.ToList())
            {
                theTransferObject.Users.Add(new DTOs.AspNetUsersDTO() {
                     Id = item.Id,
                     Email = item.Email,
                     EmailConfirmed = item.EmailConfirmed,
                     PasswordHash = item.PasswordHash,
                     SecurityStamp = item.SecurityStamp,
                     PhoneNumber = item.PhoneNumber,
                     PhoneNumberConfirmed = item.PhoneNumberConfirmed,
                     TwoFactorEnabled = item.TwoFactorEnabled,
                     LockoutEndDateUtc = item.LockoutEndDateUtc.HasValue ? item.LockoutEndDateUtc.Value : (DateTime?) null,
                     LockoutEnabled = item.LockoutEnabled,
                     AcccessFailedCount = item.AccessFailedCount,
                     UserName = item.UserName
                });

                foreach(var login in item.Logins.ToList())
                {
                    theTransferObject.UserLogins.Add(new DTOs.AspNetUserLoginsDTO()
                    {
                         UserId = login.UserId,
                         LoginProvider = login.LoginProvider,
                         ProviderKey = login.ProviderKey
                    });
                }
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(appDbContext));
            foreach(var item in roleManager.Roles.ToList())
            {
                theTransferObject.Roles.Add(new DTOs.AspNetRolesDTO()
                {
                    Id = item.Id,
                    Name = item.Name
                });

                foreach(var ruser in item.Users)
                {
                    theTransferObject.UserRoles.Add(new DTOs.AspNetUserRolesDTO()
                    {
                        UserId = ruser.UserId,
                        RoleId = ruser.RoleId
                    });
                }
            }

            XmlSerializer serializer = new XmlSerializer(typeof(DTOs.AspNetUserDataDTO));
            var xml = "";
            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"");
                    serializer.Serialize(writer, theTransferObject);
                    xml = sww.ToString();
                }
            }
            return xml;
        }

        public bool ImportAspNetUserXML(DTOs.AspNetUserDataDTO content)
        {
            var appDbContext = new Models.ApplicationDbContext();
            var userManager = new UserManager<Models.ApplicationUser>(new UserStore<Models.ApplicationUser>(appDbContext));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(appDbContext));

            foreach (var role in content.Roles)
            {
                if (!roleManager.RoleExists(role.Name))
                {
                    roleManager.Create(new IdentityRole() { Name = role.Name });
                }
            }

            foreach (var user in content.Users)
            {
                var existingUser = userManager.FindById(user.Id);
                if (existingUser == null)
                {
                    var createRes = userManager.Create(new Models.ApplicationUser()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        LockoutEnabled = user.LockoutEnabled,
                        AccessFailedCount = user.AcccessFailedCount,
                        LockoutEndDateUtc = user.LockoutEndDateUtc.HasValue ? user.LockoutEndDateUtc : null,
                        PasswordHash = user.PasswordHash,
                        PhoneNumber = user.PhoneNumber,
                        PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                        TwoFactorEnabled = user.TwoFactorEnabled,
                        UserName = user.UserName
                    });

                    if (createRes.Succeeded)
                    {

                    }
                }
                else
                {
                    existingUser.Email = user.Email;
                    existingUser.EmailConfirmed = user.EmailConfirmed;
                    existingUser.LockoutEnabled = user.LockoutEnabled;
                    existingUser.AccessFailedCount = user.AcccessFailedCount;
                    existingUser.LockoutEnabled = user.LockoutEnabled;
                    existingUser.LockoutEndDateUtc = user.LockoutEndDateUtc.HasValue ? user.LockoutEndDateUtc : null;
                    existingUser.PasswordHash = user.PasswordHash;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    existingUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                    existingUser.TwoFactorEnabled = user.TwoFactorEnabled;
                    existingUser.UserName = user.UserName;
                    
                }
            }

            List<IdentityUserLogin> userlogins = new List<IdentityUserLogin>();
            foreach (var login in content.UserLogins)
            {
                userlogins.Add(new IdentityUserLogin()
                {
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                    UserId = login.UserId
                });
            }

            return false;
        }
    }

   
}