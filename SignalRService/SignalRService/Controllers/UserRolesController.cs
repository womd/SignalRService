using SignalRService.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : BaseController
    {
        private Models.ApplicationDbContext appDbContext = new Models.ApplicationDbContext();


        public UserRolesController()
        {
           
        }

        public ActionResult Index()
        {
            return View();
        }

        #region roles-table

        public JsonResult RolesList()
        {
            var dbRoles = appDbContext.Roles.ToList();
            List<ViewModels.RoleViewModel> resList = new List<ViewModels.RoleViewModel>();
            foreach(var role in dbRoles)
            {
                resList.Add(new ViewModels.RoleViewModel() { Id = role.Id, Name = role.Name });
            }
            return Json(new { Result = "OK", Records = resList });
        }

        public JsonResult RolesCreate(ViewModels.RoleViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidData") });

            try
            {
                appDbContext.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name
                });
                appDbContext.SaveChanges();
                return Json(new { Result = "OK", Message = "data saved.." });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult RolesUpdate(ViewModels.RoleViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidData") });

            try
            {
                var dbObj = appDbContext.Roles.FirstOrDefault(ln => ln.Id == model.Id);
                dbObj.Name = model.Name;
                appDbContext.SaveChanges();
                return Json(new { Result = "OK", Message = "data saved.." });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult RolesDelete(string Id)
        {
            try
            {
                var dbObj = appDbContext.Roles.FirstOrDefault(ln => ln.Id == Id);
                appDbContext.Roles.Remove(dbObj);
                appDbContext.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region users2roles-table

        public JsonResult User2RolesList()
        {
            try
            {
                List<ViewModels.UserRolesViewModel> resList = new List<ViewModels.UserRolesViewModel>();
                foreach (var user in appDbContext.Users)
                {
                    List<ViewModels.RoleViewModel> uroles = new List<ViewModels.RoleViewModel>();
                    foreach(var role in user.Roles)
                    {
                        var xrole = appDbContext.Roles.FirstOrDefault(ln => ln.Id == role.RoleId);
                        uroles.Add(new ViewModels.RoleViewModel() { Id = xrole.Id, Name = xrole.Name });
                    }

                    resList.Add(new ViewModels.UserRolesViewModel()
                    {
                        IdentityName = user.UserName,
                        IdentityId = user.Id,
                        Roles = uroles
                    });
                }
                return Json(new { Result = "OK", Records = resList });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
         
        }

        public JsonResult User2RolesCreate(string IdentityName, string Roles)
        {
            if (!ModelState.IsValid)
                return Json(new { Result = "ERROR", Message = BaseResource.Get("InvalidData") });

            try
            {
                var user = appDbContext.Users.FirstOrDefault(ln => ln.UserName == IdentityName);
                if(user == null)
                    return Json(new { Result = "ERROR", Message = BaseResource.Get("UserNotFound") });

                var role = appDbContext.Roles.FirstOrDefault(ln => ln.Name == Roles);
                if(role == null)
                    return Json(new { Result = "ERROR", Message = BaseResource.Get("RoleNotFound") });

                user.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole() { RoleId = role.Id, UserId = user.Id });
                appDbContext.SaveChanges();

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public JsonResult User2RolesDelete(string IdentityId)
        {
            try
            {
                var user = appDbContext.Users.FirstOrDefault(ln => ln.Id == IdentityId);
                var uroles = user.Roles.ToList();
                foreach(var item in uroles)
                {
                    user.Roles.Remove(item);
                }
                appDbContext.SaveChanges();
                return Json(new { Result = "OK", Message = "removed all roles..." });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion
    }
}