using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SignalRService.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace SignalRService.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : BaseController
    {
        private Models.ApplicationDbContext appDbContext = new Models.ApplicationDbContext();
        private UserManager<Models.ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private Repositories.UserRepository _userRepository;

        public UserRolesController()
        {
            _userManager = new UserManager<Models.ApplicationUser>(new UserStore<Models.ApplicationUser>(appDbContext));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(appDbContext));
            _userRepository = new Repositories.UserRepository(new DAL.ServiceContext());
        }

        public ActionResult Index()
        {
            return View();
        }

        #region roles-table

        public JsonResult RolesList()
        {
            
            List<ViewModels.RoleViewModel> resList = new List<ViewModels.RoleViewModel>();
            foreach(var role in _roleManager.Roles)
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
                var rcreateres = _roleManager.Create(new IdentityRole() { Name = model.Name });
                if (rcreateres.Succeeded)
                {
                    var newrole = _roleManager.FindByName(model.Name);
                    return Json(new { Result = "OK", Record = new ViewModels.RoleViewModel() { Id = newrole.Id, Name = newrole.Name }   });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = rcreateres.Errors.First().ToString() });
                }
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
                var role = _roleManager.FindById(model.Id);
                role.Name = model.Name;
                _roleManager.Update(role);

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
                var role = _roleManager.FindById(Id);
                _roleManager.Delete(role);
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


                foreach (var dbuser in appDbContext.Users)
                {
                    var user = _userManager.FindById(dbuser.Id);

                    List<ViewModels.RoleViewModel> uroles = new List<ViewModels.RoleViewModel>();
                    foreach(var role in user.Roles)
                    {
                        var xrole = _roleManager.FindById(role.RoleId);
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
                if (!_roleManager.RoleExists(Roles))
                    return Json(new { Result = "ERROR", Message = BaseResource.Get("Invalid Role...") });

                var role = _roleManager.Roles.FirstOrDefault(ln => ln.Name == Roles);
                var user = _userManager.FindByName(IdentityName);
                List<ViewModels.RoleViewModel> retlist = new List<ViewModels.RoleViewModel>();
                var addRes = _userManager.AddToRole(user.Id, role.Name);
                if (addRes.Succeeded)
                {
                    retlist.Add(new ViewModels.RoleViewModel() { Id = role.Id, Name = role.Name });
                    ViewModels.UserRolesViewModel ret = new ViewModels.UserRolesViewModel() { IdentityId = user.Id, IdentityName = user.UserName, Roles = retlist };
                    return Json(new { Result = "OK", Record = ret });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message =  addRes.Errors.First().ToString() });
                }

               
                
                
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
                var user = _userManager.FindById(IdentityId);
                List<IdentityUserRole> toremove = new List<IdentityUserRole>();
                foreach(var item in user.Roles)
                {
                    toremove.Add(item);
                }

                foreach(var ritem in toremove)
                {
                    user.Roles.Remove(ritem);

                }
                _userManager.Update(user);

                return Json(new { Result = "OK", Message = "removed all roles..." });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public bool ImportAspNetUserXML()
        {
            DTOs.AspNetUserDataDTO xmlcontents = new DTOs.AspNetUserDataDTO();
            XmlSerializer serializer = new XmlSerializer(typeof(DTOs.AspNetUserDataDTO));
            foreach (string pfile in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[pfile];
               // string fileName = file.FileName;
               // fileName = Server.MapPath("~/uploads/" + fileName);
                // file.InputStream .SaveAs(fileName);
                using (XmlReader reader = XmlReader.Create(file.InputStream))
                {
                    xmlcontents = (DTOs.AspNetUserDataDTO) serializer.Deserialize(reader);
                }
            }

              _userRepository.ImportAspNetUserXML(xmlcontents);
            

            return true;
        }

        [Authorize(Roles = "Admin")]
        public FileResult ExportAspNetUserXML()
        {
            var xmldata = _userRepository.GetAspNetUsersXML();
            string filename = "AspNetUsers_Export_" + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToLongTimeString() + ".xml";
            return File(Encoding.ASCII.GetBytes(xmldata), "text/plain", filename);
        }
    }
}