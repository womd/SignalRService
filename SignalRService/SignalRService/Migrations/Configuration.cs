using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.DAL;
using System.Data.Entity.Migrations;

namespace SignalRService.Migrations
{
    public class Configuration : DbMigrationsConfiguration<ServiceContext>
    {
        public Configuration()
        {
            //AutomaticMigrationsEnabled = true;
            CommandTimeout = Int32.MaxValue;
            ContextKey = "SignalRService.DAL.ServiceContext";
        }

        protected override void Seed(ServiceContext context)
        {
            var dbuser = context.AccountProperties.Add(new Models.AccountPropertiesModel()
            {
                UserId = "testuser"
            });

            context.ServiceSettings.Add(new Models.ServiceSettingModel()
            {
                ServiceName = "testname",
                ServiceUrl = "testurl",
                ServiceType = Enums.EnumServiceType.OrderService,
                Owner = dbuser
                
            });
        }
    }
}