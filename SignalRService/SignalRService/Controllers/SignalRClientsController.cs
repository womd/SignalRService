using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Controllers
{
    public class SignalRClientsController : Controller
    {
        // GET: SignalRClients
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ClientList()
        {
            try
            {
                List<Models.UserDataViewModel> clients = DAL.SignalRConnections.Instance.List();
                return Json(new { Result = "OK", Records = clients });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult DeleteClient(string ConnectionId)
        {
            try
            {
                DAL.SignalRConnections.Instance.Remove(ConnectionId);

                //todo:
                //- send delete to other clients
                //- close / block the deleted client

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}