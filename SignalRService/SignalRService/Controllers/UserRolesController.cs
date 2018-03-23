using SignalRService.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Controllers
{
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
    }
}