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