using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
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

        public ViewModels.MiningRoomViewModel GetOverview(int MiningRoomId)
        {
            var dbRoom = db.MiningRooms.FirstOrDefault(ln => ln.Id == MiningRoomId);
            if (dbRoom == null)
                return new MiningRoomViewModel();

            var minerClientId = dbRoom.ServiceSetting.MinerConfiguration.ClientId;
            var TotalHash = GetClientTotalHash(minerClientId);
            var TotalReward = GetClientReward(minerClientId);

            return new MiningRoomViewModel() {
                HashesTotal = TotalHash,
                XMR_Mined = TotalReward
            };
        }

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