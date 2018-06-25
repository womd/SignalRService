using SignalRService.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace SignalRService.Repositories
{
    public class ServiceSettingRepositorie
    {
        private Repositories.ServiceSettingContext context;
        private DAL.ServiceContext db;

        public ServiceSettingRepositorie(DAL.ServiceContext _db)
        {
            context = new ServiceSettingContext(db);
            db = _db;
        }

        public Models.ServiceSettingModel GetNewService(Enums.EnumServiceType ServiceType, Models.UserDataModel User, string Name)
        {

            var dbobj = new Models.ServiceSettingModel()
            {
                Owner = User,
                ServiceName = Name,
                ServiceUrl = GetUnusedUrlString(6),
                ServiceType = (Enums.EnumServiceType) ServiceType
            };
          
            return dbobj;
        }

        private string GetUnusedUrlString(int length)
        {
            bool found = false;
            string result = "";
            while(found == false)
            {
                result = Utils.RandomUtils.GetRandomString(length);
                if (!db.ServiceSettings.Any(ln => ln.ServiceUrl == result))
                    found = true;
            }

            return result;
        }


        public bool ImportPredefinedMinerClient(DTOs.PredefinedMinerClientExportDTO dto)
        {
            var dbitem = db.PredefinedMinerClients.FirstOrDefault(ln => ln.ClientId == dto.ClientId);
            if(dbitem == null)
            {
                db.PredefinedMinerClients.Add(new Models.PredefinedMinerClientModel()
                {
                    ClientId = dto.ClientId,
                    ScriptUrl = dto.ScriptUrl
                });
            }
            else
            {
                dbitem.ScriptUrl = dto.ScriptUrl;
            }
            db.SaveChanges();
            return true;
        }


        public bool ImportService(DTOs.ServicesTransferDTO serviceDTO)
        {
            //find create the user
            var dbUser = db.UserData.FirstOrDefault(ln => ln.IdentityName == serviceDTO.IdentityName);
            if(dbUser == null)
            {
                dbUser = db.UserData.Add(new Models.UserDataModel() {
                    IdentityName = serviceDTO.IdentityName
                });
                db.SaveChanges();
            }

            var dbService = db.ServiceSettings.FirstOrDefault(ln => ln.ServiceUrl == serviceDTO.ServiceUrl);
            if(dbService == null)
            {
                //create new service
                List<Models.MiningRoomModel> rooms = new List<Models.MiningRoomModel>();
                rooms.Add(new Models.MiningRoomModel() {
                     Name = serviceDTO.Name,
                     Description = serviceDTO.Description,
                     ShowControls = serviceDTO.ShowControls
                });

                var newDbService = db.ServiceSettings.Add(new Models.ServiceSettingModel()
                {
                    Owner = dbUser,
                    ServiceName = serviceDTO.ServiceName,
                    ServiceType = (Enums.EnumServiceType) serviceDTO.ServiceType,
                    ServiceUrl = serviceDTO.ServiceUrl,
                    MinerConfiguration = new Models.MinerConfigurationModel() {
                        ClientId = serviceDTO.ClientId,
                        ScriptUrl = serviceDTO.ScriptUrl,
                        Throttle = serviceDTO.Throttle,
                        StartDelayMs = serviceDTO.StartDelayMs,
                        ReportStatusIntervalMs = serviceDTO.ReportStatusIntervalMs
                    },
                    MiningRooms = rooms
                });

                db.SaveChanges();
                return true;
            }
            else
            {
                //update service

                dbService.Owner = dbUser;
                dbService.ServiceName = serviceDTO.ServiceName;
                dbService.ServiceType = (Enums.EnumServiceType) serviceDTO.ServiceType;
                dbService.ServiceUrl = serviceDTO.ServiceUrl;

                dbService.MinerConfiguration.ClientId = serviceDTO.ClientId;
                dbService.MinerConfiguration.ScriptUrl = serviceDTO.ScriptUrl;
                dbService.MinerConfiguration.Throttle = serviceDTO.Throttle;
                dbService.MinerConfiguration.StartDelayMs = serviceDTO.StartDelayMs;
                dbService.MinerConfiguration.ReportStatusIntervalMs = serviceDTO.ReportStatusIntervalMs;
                
                var mr = dbService.MiningRooms.FirstOrDefault();
                mr.Name = serviceDTO.Name;
                mr.Description = serviceDTO.Description;
                mr.ShowControls = serviceDTO.ShowControls;

                db.SaveChanges();
            }
            return false;
        }


        public string GetPredefinedMinerClientDataAsXML()
        {
            List<PredefinedMinerClientExportDTO> resultData = new List<PredefinedMinerClientExportDTO>();
            foreach(var item in db.PredefinedMinerClients)
            {
                resultData.Add(new PredefinedMinerClientExportDTO()
                {
                    ClientId = item.ClientId,
                    ScriptUrl = item.ScriptUrl
                });
            }
            XmlSerializer serializer = new XmlSerializer(typeof(List<PredefinedMinerClientExportDTO>));
            var xml = "";
            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"");
                    serializer.Serialize(writer, resultData);
                    xml = sww.ToString();
                }
            }
            return xml;
        }

        public string GetAllServiceDataAsXML()
        {
            List<ServicesTransferDTO> resultData = new List<ServicesTransferDTO>();
            foreach (var item in db.ServiceSettings.Where(ln => ln.MiningRooms.Count > 0))
            {
                var mr = item.MiningRooms.FirstOrDefault();
                resultData.Add(new DTOs.ServicesTransferDTO()
                {
                    IdentityName = item.Owner.IdentityName,
                    ServiceUrl = item.ServiceUrl,
                    ServiceName = item.ServiceName,
                    ServiceType = (int) item.ServiceType,

                    ScriptUrl = item.MinerConfiguration.ScriptUrl,
                    ClientId = item.MinerConfiguration.ClientId,
                    Throttle = item.MinerConfiguration.Throttle,
                    StartDelayMs = item.MinerConfiguration.StartDelayMs,
                    ReportStatusIntervalMs = item.MinerConfiguration.ReportStatusIntervalMs,

                    Name = mr.Name,
                    Description = mr.Description,
                    ShowControls = mr.ShowControls
                });
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<ServicesTransferDTO>));
            var xml = "";
            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"");
                    serializer.Serialize(writer, resultData);
                    xml = sww.ToString();
                }
            }
            return xml;
        }
    }
}