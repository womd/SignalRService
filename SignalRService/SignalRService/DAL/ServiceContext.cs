using Microsoft.AspNet.Identity.EntityFramework;
using SignalRService.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SignalRService.DAL
{
    public class ServiceContext : DbContext
    {

        public ServiceContext() : base("ServiceContext")
        {

        }

        public DbSet<AccountPropertiesModel>AccountProperties { get; set; }
        public DbSet<ServiceSettingModel>ServiceSettings { get; set; }
        public DbSet<MinerConfigurationModel>MinerConfiurationModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}