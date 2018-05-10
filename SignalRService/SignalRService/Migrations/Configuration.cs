using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalRService.DAL;
using System.Data.Entity.Migrations;
using SignalRService.Models;

namespace SignalRService.Migrations
{
    public class Configuration : DbMigrationsConfiguration<ServiceContext>
    {
        public Configuration()
        {
            //AutomaticMigrationsEnabled = true;
            CommandTimeout = Int32.MaxValue;
            ContextKey = "SignalRService.DAL.ServiceContext";
            AutomaticMigrationsEnabled = true;

        }

        protected override void Seed(ServiceContext context)
        {
            //base.Seed(context);
            //here it would seed things on db-migration
            // moved all seeding to homecontroller until further refactoring
        }
    }
}