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
using SignalRService.DTOs;
using Newtonsoft.Json;
using SignalRService.Localization;
using System.Globalization;

namespace SignalRService.Implementation
{
    public class MiningRoomCSECoin : IMiningRoom, IServiceImport
    {
        private DAL.ServiceContext db;
        string apiUrl;
        string cmpPrivate;
        string cmpPublic;
        private Repositories.MinerRoomRepository miningRoomRepo;
        private Repositories.UserRepository userRepo;
        private Repositories.ServiceSettingRepositorie serviceRepo;
        private Repositories.JSECoinMinerRepository minerRepo;


        public MiningRoomCSECoin()
        {
            db = new DAL.ServiceContext();
            apiUrl = (string) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.JSECoinApiUrl);
            cmpPrivate = (string) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.JSECoinPrivateKey);
            cmpPublic = (string) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.JSECoinPublicKey);
            miningRoomRepo = new Repositories.MinerRoomRepository(db);
            userRepo = new Repositories.UserRepository(db);
            serviceRepo = new Repositories.ServiceSettingRepositorie(db);
            minerRepo = new Repositories.JSECoinMinerRepository(db);
        }

        #region interface methods

        public GeneralHubResponseObject CreateMiningRoom(System.Security.Principal.IPrincipal User, MiningRoomRequesObject mrRequest)
        {
            GeneralHubResponseObject result = new GeneralHubResponseObject();

            if (!User.Identity.IsAuthenticated)
            {
                result.Success = false;
                result.ErrorMessage = SignalRService.Localization.BaseResource.Get("MsgLoginFirst");
                return result;
            }
            string toParse = ((dynamic)mrRequest.CommandData).CoinType;
            if (!Enum.TryParse(toParse, out Enums.EnumMiningRoomType coinType))
            {
                result.ErrorMessage = "Invalid type given";
                result.Success = false;
                return result;
            }

            //if it's not this implementation, use coinimp
            switch (coinType)
            {
                case Enums.EnumMiningRoomType.CoinIMP:
                    var roomImplementation = Factories.MiningRoomFactory.GetImplementation(Enums.EnumMiningRoomType.CoinIMP);
                    return roomImplementation.CreateMiningRoom(User, mrRequest);
                    
                case Enums.EnumMiningRoomType.JSECoin:

                    break;
                default:
                    break;
            }



           

            string roomName = ((dynamic)mrRequest.CommandData).RoomName;
            string clientId = ((dynamic)mrRequest.CommandData).ClientId;
            string siteId = ((dynamic)mrRequest.CommandData).SiteId;
            string subId = ((dynamic)mrRequest.CommandData).SubId;

            var mrMinLength = (int)Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.MiningRoomNameMinLength);
            var MrMaxLength = (int)Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.MiningRoomNameMaxLength);

            if (roomName.Length > MrMaxLength || roomName.Length < mrMinLength)
            {
                result.ErrorMessage = "Name has to be from " + mrMinLength + " to " + MrMaxLength + " characters.";
                result.Success = false;
                return result;
            }

            bool isDangerous = Utils.ValidationUtils.IsDangerousString(roomName, out int badIdx);

            if (isDangerous)
            {
                result.ErrorMessage = "Invalid character in Name";
                result.Success = false;
                return result;
            }

            var clIdMinLength = (int)Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.JSECoinClientIdMinLength);
            var clIdMaxLength = (int)Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.JSECoinClientIdMaxLength);

            if (clientId.Length > clIdMaxLength || clientId.Length < clIdMinLength)
            {
                result.ErrorMessage = "ClientId has to be from " + clIdMinLength + " to " + clIdMaxLength + " characters.";
                result.Success = false;
                return result;
            }

            var user = userRepo.GetDbUser(User.Identity.Name);

            var newService = serviceRepo.GetNewService(Enums.EnumServiceType.CrowdMinerCoinIMP, user, roomName);

            var defaultMinerConf = minerRepo.GetDefaultMinerConfig();
            var newMinerConf = minerRepo.GetNewMinerConfig(clientId, siteId, subId);

            newService.JSECoinMinerConfiguration = newMinerConf;

            var theRoom = miningRoomRepo.CreateRoom(newService);

        //    db.PredefinedMinerClients.Remove(predefClient);
            db.SaveChanges();

            result.Success = true;
            result.ResponseData = theRoom.ServiceSetting.ServiceUrl;
            return result;
        }
        
        public dynamic GetOverview(int MiningRoomId)
        {
            int tresholdSec = (int) Utils.GeneralSettingsUtils.GetSettingValue(Enums.EnumGeneralSetting.JSEAPITresholdSec);
            ViewModels.MiningRoomJSECoinViewModel result = new MiningRoomJSECoinViewModel();

            //var md = new MarkdownDeep.Markdown();
            //md.NewWindowForExternalLinks = true;
            //md.NewWindowForLocalLinks = true;
            //md.SafeMode = true;



            var dbRoom = db.MiningRooms.FirstOrDefault(ln => ln.Id == MiningRoomId);
            if (dbRoom == null)
                return result;

            var currGroup = dbRoom.ServiceSetting.ServiceUrl.ToLower();
            var currGroupDb = db.SignalRGroups.FirstOrDefault(ln => ln.GroupName == currGroup);

            var cacheRes = Utils.JSECoinMiningRoomInfoCache.GetItem(MiningRoomId, tresholdSec);
            if (cacheRes != null)
                return cacheRes;
            
            var TotalBalance = GetClientBalance(dbRoom.ServiceSetting.JSECoinMinerConfiguration.SiteId);
           

            result.Id = dbRoom.Id;
            result.Name = dbRoom.Name;
            result.Balance = TotalBalance;

            Utils.JSECoinMiningRoomInfoCache.AddItem(MiningRoomId, result);

            return result;
        }

       

        public void SendRoomInfoUpdateToClients(dynamic vm, string signalRGroup)
        {
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Group(signalRGroup.ToLower()).updateMinigRoomOverView(vm);
        }

        public void SendRoomInfoUpdateToClient(dynamic vm, string connectionId)
        {
            GlobalHost.ConnectionManager.GetHubContext<ServiceHub>().Clients.Client(connectionId).updateMinigRoomOverView(vm);
        }
    
        public DTOs.GeneralHubResponseObject ProcessIncoming(DTOs.GeneralHubRequestObject Request)
        {
            GeneralHubResponseObject result = new GeneralHubResponseObject();

            var jconf = JsonConvert.SerializeObject(Request.RequestData);
            var mrRequest = JsonConvert.DeserializeObject<MiningRoomRequesObject>(jconf);
           

            switch (mrRequest.Command)
            {
                case "GetRoomOverview":
                    var overviewData = GetOverview(mrRequest.MiningRoomId);
                    result.Success = true;
                    result.ResponseData = overviewData;
                    break;

             
                case "ToggleControls":
                    var dbMiningRoomx = db.MiningRooms.FirstOrDefault(ln => ln.Id == mrRequest.MiningRoomId);
                    if (dbMiningRoomx.ServiceSetting.Owner == Request.User || Request.User.IsInRole("Admin"))
                    {
                        bool isOn = bool.Parse(mrRequest.CommandData.ToString());
                        dbMiningRoomx.ShowControls = isOn;
                        db.SaveChanges();

                        var vm = GetOverview(mrRequest.MiningRoomId);
                        SendRoomInfoUpdateToClients(vm, dbMiningRoomx.ServiceSetting.ServiceUrl.ToLower());
                    }
                    break;

                case "CreateRoom":

                    result = CreateMiningRoom(Request.User, mrRequest);
                    break;

                case "UpdateDescription":
                    if (!Request.User.Identity.IsAuthenticated)
                    {
                        result.Success = false;
                        result.ErrorMessage = SignalRService.Localization.BaseResource.Get("MsgLoginFirst");
                        break;
                    }

                    var dbRoom = db.MiningRooms.FirstOrDefault(ln => ln.Id == mrRequest.MiningRoomId);
                    if(dbRoom == null)
                    {
                        result.Success = false;
                        result.ErrorMessage = "Invalid RoomId";
                    }

                    if (!Utils.ServiceUtils.IsServiceOwner(dbRoom.ServiceSetting.ID, Request.User.Identity.Name))
                    {
                        if (!Request.User.IsInRole("Admin"))
                        {
                            result.Success = false;
                            result.ErrorMessage = "No Permission";
                            break;
                        }
                    }

                    string contentData = ((dynamic)mrRequest.CommandData).Content;
                    string cultureName = ((dynamic)mrRequest.CommandData).Culture;

                    var localizationKey = GetDescriptionKeyForRoom(dbRoom.Id);
                    Repositories.LocalizationRepository localizationRepo = new Repositories.LocalizationRepository(db);
                    if(localizationRepo.Exists(localizationKey, cultureName))
                    {
                        var dbItem = localizationRepo.Get(localizationKey, cultureName);
                        dbItem.Value = contentData;
                        localizationRepo.Update(dbItem, Request.User.Identity.Name);

                        Localization.UiResources.Instance.removeFromCache(localizationKey, cultureName);

                        result.Success = true;
                        result.Message = "description updated successfuly";
                    }
                    else
                    {
                        localizationRepo.Create(new Models.LocalizationModel() {
                             CreationDate = DateTime.Now,
                             Archived = false,
                             Culture = cultureName,
                             Key = localizationKey,
                             LastModDate = DateTime.Now,
                             ModUser = Request.User.Identity.Name,
                             TranslationStatus = Enums.EnumTranslationStatus.Undefined,
                             Value = contentData,
                             WasHit = false
                        }, Request.User.Identity.Name);

                        result.Success = true;
                        result.Message = "creating localizationItem success.";
                    }

                    break;
                default:
                    break;
            }

            return result;
        }

        private string GetDescriptionKeyForRoom(int Id)
        {
            return "MiningRoomRoomId_" + Id + "_description";
        }

        public void ImportSource(string xmlFile)
        {
           
        }


        #endregion


        private int GetClientBalance(string SiteId)
        {
            string apiUrlComplete = apiUrl + "/v1.7/balance/auth/" + SiteId + "/";

            try
            {
                WebRequest request = WebRequest.Create(apiUrlComplete);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Headers.Add("Authorization: " + cmpPrivate);
                request.ContentType = "application/json";
                ((HttpWebRequest) request).UserAgent = ".NET Framework HttpWebRequest womd";
                WebResponse response = request.GetResponse();
               
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                
                string responseFromServer = reader.ReadToEnd();
                dynamic responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer);

                reader.Close();
                response.Close();

                return responseJson.balance;
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