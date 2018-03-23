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

        public DbSet<SignalRConnectionModel>SignalRConnections { get; set; }
        public DbSet<UserDataModel>UserData { get; set; }
        public DbSet<ServiceSettingModel>ServiceSettings { get; set; }
        public DbSet<MinerConfigurationModel>MinerConfiurationModels { get; set; }
        public DbSet<MinerStatusModel>MinerStatus{ get; set; }
        public DbSet<OrderModel>Orders { get; set; }
        public DbSet<ProductModel>Products { get; set; }
        public DbSet<OrderItemModel>OrderItems { get; set; }
        public DbSet<LocalizationModel> Localization { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }

        public void AddConnection(string connectionId, string refererUrl, string remoteIp, string identityName)
        {
            var dbObjUd = UserData.FirstOrDefault(ln => ln.IdentityName == identityName);
            if (dbObjUd == null)
            {
                dbObjUd = UserData.Add(new Models.UserDataModel() { IdentityName = identityName });

                SignalRConnections.Add(new Models.SignalRConnectionModel()
                {
                    SignalRConnectionId = connectionId,
                    ConnectionState = Enums.EnumSignalRConnectionState.Connected,
                    RefererUrl = refererUrl,
                    RemoteIp = remoteIp,
                    User = dbObjUd
                });
            }
            else
            {
                SignalRConnections.Add(new Models.SignalRConnectionModel()
                {
                    SignalRConnectionId = connectionId,
                    ConnectionState = Enums.EnumSignalRConnectionState.Connected,
                    RefererUrl = refererUrl,
                    RemoteIp = remoteIp,
                    User = dbObjUd
                });
            }
           
            SaveChanges();
        }
        public void AddConnection(string connectionId, string refererUrl, string remoteIp)
        {
            var user = UserData.FirstOrDefault(ln => ln.IdentityName == "Anonymous");
            if (user == null)
                user = UserData.Add(new Models.UserDataModel() { IdentityName = "Anonymous" });

            SignalRConnections.Add(new Models.SignalRConnectionModel()
            {
                SignalRConnectionId = connectionId,
                ConnectionState = Enums.EnumSignalRConnectionState.Connected,
                RefererUrl = refererUrl,
                RemoteIp = remoteIp,
                User = user
            });
            SaveChanges();
        }

            public void RemoveConnection(string connectionId)
        {
            var rmObj = SignalRConnections.FirstOrDefault(ln => ln.SignalRConnectionId == connectionId);
            if (rmObj != null)
            {
                SignalRConnections.Remove(rmObj);
                SaveChanges();
            }
        }

        public void UpdateConnectionState(string connectionId, Enums.EnumSignalRConnectionState state)
        {
            var dbObj = SignalRConnections.FirstOrDefault(ln => ln.SignalRConnectionId == connectionId);
            dbObj.ConnectionState = state;
            SaveChanges();
        }

        public void UpdateMinerState(Hubs.MinerStatusData data, string connectionId)
        {
            var dbObjConn = SignalRConnections.FirstOrDefault(ln => ln.SignalRConnectionId == connectionId);
            if (dbObjConn.MinerStatus.Count == 0)
            {
                MinerStatus.Add(new MinerStatusModel()
                {
                    Hashes = data.hashes,
                    Hps = data.hps,
                    IsAutoThreads = data.isAutoThreads,
                    OnMobile = data.onMobile,
                    Running = data.running,
                    Threads = data.threads,
                    Throttle = data.throttle,
                    WasmEnabled = data.wasmEnabled,
                    SignalRConnection = dbObjConn
                });
            }
            else
            {
                dbObjConn.MinerStatus.First().Hashes = data.hashes;
                dbObjConn.MinerStatus.First().Hps = data.hps;
                dbObjConn.MinerStatus.First().OnMobile = data.onMobile;
                dbObjConn.MinerStatus.First().Running = data.running;
                dbObjConn.MinerStatus.First().Threads = data.threads;
                dbObjConn.MinerStatus.First().Throttle = data.throttle;
                dbObjConn.MinerStatus.First().WasmEnabled = data.wasmEnabled;               
            }
            SaveChanges();
        }

        
       

    }
}