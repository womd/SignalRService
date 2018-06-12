﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRService.Hubs;
using SignalRService.Interfaces;
using SignalRService.ViewModels;

namespace SignalRService.Implementation
{
    public class MiningRoomBasic : IMiningRoom
    {
        private DAL.ServiceContext db;
        string apiUrl;
        string cmpPrivate;
        string cmpPublic;
       
        public MiningRoomBasic()
        {
            db = new DAL.ServiceContext();
            apiUrl = (string) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.CoinImpApiBaseUrl);
            cmpPrivate = (string) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.CoinImpPrivateKey);
            cmpPublic = (string) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.CoinImpPublicKey);

        }

        #region interface methods

        
        public ViewModels.MiningRoomViewModel GetOverview(int MiningRoomId)
        {
            int tresholdSec = (int) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.CoinImpApiCallTresholdSec);
            ViewModels.MiningRoomViewModel result = new MiningRoomViewModel();


            var cacheRes = Utils.MiningRoomInfoCache.GetItem(MiningRoomId, tresholdSec);
            if (cacheRes != null)
                return cacheRes;

           
            var dbRoom = db.MiningRooms.FirstOrDefault(ln => ln.Id == MiningRoomId);
            if (dbRoom == null)
                return result;

            var minerClientId = dbRoom.ServiceSetting.MinerConfiguration.ClientId;
            var TotalHash = GetClientTotalHash(minerClientId);
            var TotalReward = GetClientReward(minerClientId);

            var md = new MarkdownDeep.Markdown();


            result.Id = dbRoom.Id;
            result.Name = dbRoom.Name;
            result.Description = md.Transform(dbRoom.Description);
            result.DescriptionMarkdown = dbRoom.Description;
            result.HashesTotal = TotalHash;
            result.XMR_Mined = TotalReward;
            result.DataSnapshot = DateTime.Now;

            if(result.HashesTotal != 0)
                Utils.MiningRoomInfoCache.AddItem(MiningRoomId, result);

            SendRoomInfoUpdateToClients(result, dbRoom.ServiceSetting.ServiceUrl);
            return result;
        }

        public ViewModels.MiningRoomUpdateResult UpdateDescription(int MiningRoomId, string Content)
        {

            var mr = db.MiningRooms.FirstOrDefault(ln => ln.Id == MiningRoomId);
            if (mr == null)
                return new ViewModels.MiningRoomUpdateResult() { Success = false, Message = "invalid id" };

            mr.Description = Content;
            db.SaveChanges();
            return new MiningRoomUpdateResult() { Success = true };
        }

        public void SendRoomInfoUpdateToClients(MiningRoomViewModel vm, string signalRGroup)
        {
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(signalRGroup.ToLower()).updateMinigRoomOverView(vm);
        }

        public void SendRoomInfoUpdateToClient(MiningRoomViewModel vm, string connectionId)
        {
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Client(connectionId).updateMinigRoomOverView(vm);
        }
    
        #endregion

       
        private int GetClientTotalHash(string MinerClientId)
        {
            string apiUrlComplete = apiUrl + "hashes?site-key=" + MinerClientId + "&public=" + cmpPublic + "&private=" + cmpPrivate;

            try
            {
                WebRequest request = WebRequest.Create(apiUrlComplete);
                request.Credentials = CredentialCache.DefaultCredentials;
                ((HttpWebRequest) request).UserAgent = ".NET Framework HttpWebRequest womd";
                WebResponse response = request.GetResponse();
               
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                
                string responseFromServer = reader.ReadToEnd();
                dynamic responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer);

                reader.Close();
                response.Close();

                return responseJson.message;
            }
            catch (Exception ex)
            {

                Utils.SimpleLogger logger = new Utils.SimpleLogger();
                logger.Error(ex.Message);
            }
            return 0;
        }

        private decimal GetClientReward(string MinerClientId)
        {
            string apiUrlComplete = apiUrl + "reward?site-key=" + MinerClientId + "&public=" + cmpPublic + "&private=" + cmpPrivate;

            try
            {
                WebRequest request = WebRequest.Create(apiUrlComplete);
                request.Credentials = CredentialCache.DefaultCredentials;
                ((HttpWebRequest) request).UserAgent = ".NET Framework HttpWebRequest womd";
                WebResponse response = request.GetResponse();

                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);


                string responseFromServer = reader.ReadToEnd();
                dynamic responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer);

                reader.Close();
                response.Close();

                return responseJson.message;
            }
            catch (Exception ex)
            {

                Utils.SimpleLogger logger = new Utils.SimpleLogger();
                logger.Error(ex.Message);
            }
            return 0;
        }

    }
}