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
                ServiceType = dbmodel.ServiceType.ToString()
            };
        }

        
    }
}