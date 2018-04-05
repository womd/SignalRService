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
    [Authorize(Roles = "Admin")]
    public class SignalRClientsController : BaseController
    {
        private DAL.ServiceContext db = new DAL.ServiceContext();

        public ActionResult Index()
        {
            var signalRclientsBaseViewModel = new ViewModels.SignalRClientsBaseViewModel()
            {
                signalRBaseConfigurationViewModel = new SignalRBaseConfigurationViewModel()
                {
                    SinalRGroup = "signalrclientsindex"
                }
            };
            return View("Index", signalRclientsBaseViewModel);
        }

        public JsonResult ClientList()
        {
            try
            {

                List<SignalRConnectionModel> sigRClients = db.SignalRConnections.ToList();
                List<UserDataSignalRTableViewModel> uvms = new List<UserDataSignalRTableViewModel>();
                foreach(var sigRC in sigRClients)
                {
                    var mstat = sigRC.MinerStatus.FirstOrDefault();

                    uvms.Add(new UserDataSignalRTableViewModel() {
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
                db.SaveChanges();
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult ClientUpdate(UserDataSignalRTableViewModel model)
        {
            try
            {

                var dbConn = db.SignalRConnections.FirstOrDefault(ln => ln.SignalRConnectionId == model.ConnectionId);
                var minerstat = dbConn.MinerStatus.FirstOrDefault();
                if(minerstat != null)
                {
                    if (minerstat.Running && model.MinerIsRunning)
                    {
                        Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Client(model.ConnectionId).miner_setThrottle(model.MinerThrottle);
                        return Json(new { Result = "OK", Message = "throttle update sent.." });
                    }
                    else
                    {
                        if (!minerstat.Running && model.MinerIsRunning)
                        {
                            Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Client(model.ConnectionId).miner_start();
                            return Json(new { Result = "OK", Message = "miner started..." });
                        }
                        else
                        {
                            Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Client(model.ConnectionId).miner_stop();
                            return Json(new { Result = "OK", Message = "miner stopped.." });
                        }

                    }
                }
                else
                {
                    if (model.MinerIsRunning)
                    {
                        Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Client(model.ConnectionId).miner_start();
                        return Json(new { Result = "OK", Message = "miner started..." });
                    }
                    else
                    {
                        Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Client(model.ConnectionId).miner_stop();
                        return Json(new { Result = "OK", Message = "miner stopped..." });
                    }
                }

            }
            catch(Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}