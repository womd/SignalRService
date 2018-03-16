using Microsoft.AspNet.SignalR;
using SignalRService.Hubs;
using SignalRService.Models;
using SignalRService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SignalRService.Controllers
{
   
    public class SignalRClientsController : Controller
    {
        private DAL.ServiceContext db = new DAL.ServiceContext();

        // GET: SignalRClients
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ClientList()
        {
            try
            {
                List<SignalRConnectionModel> sigRClients = db.SignalRConnections.ToList();
                List<UserDataViewModel> uvms = new List<UserDataViewModel>();
                foreach(var sigRC in sigRClients)
                {
                    var mstat = sigRC.MinerStatus.FirstOrDefault();

                    uvms.Add(new UserDataViewModel() {
                        ConnectionId = sigRC.SignalRConnectionId,
                        ConnectionState = sigRC.ConnectionState.ToString(),
                        NrOfGroups = sigRC.Groups.Count,
                        MinerHps = mstat != null ? mstat.Hps : 0,
                        MinerIsMobile = mstat != null ? mstat.OnMobile : false,
                        MinerIsRunning = mstat != null ? mstat.Running : false,
                        MinerThrottle = mstat != null ? mstat.Throttle : 1.0f,
                        RefererUrl = sigRC.RefererUrl,
                        RemoteIp = sigRC.RemoteIp,
                        UserName = sigRC.User != null ? sigRC.User.IdentityName : "--"
                    });
                }

                return Json(new { Result = "OK", Records = uvms });
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
                var rmObj = db.SignalRConnections.FirstOrDefault(ln => ln.SignalRConnectionId == ConnectionId);
                db.SignalRConnections.Remove(rmObj);
        
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

        public JsonResult ClientUpdate(UserDataViewModel model)
        {
            try
            {
                GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Client(model.ConnectionId).miner_setThrottle(model.MinerThrottle);
                return Json(new { Result = "OK", Message = "throttle update sent" });
            }
            catch(Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}