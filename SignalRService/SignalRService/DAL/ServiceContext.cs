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
        public DbSet<OrderJournalModel>OrderJournal { get; set; }
        public DbSet<ProductModel>Products { get; set; }
        public DbSet<OrderItemModel>OrderItems { get; set; }
        public DbSet<LocalizationModel> Localization { get; set; }
        public DbSet<StripeSettingsModel> StripeSettings { get; set; }
        public DbSet<ProductImportConfigurationModel> ProductImportConfigurations { get; set; }
        public DbSet<ProductImportModel> ProductTmpImport { get; set; }
        public DbSet<GeneralSettingsModel>GeneralSettings { get; set; }
        public DbSet<LuckyGameSettingsModel>LuckyGameSettings { get; set; }
        public DbSet<LuckyGameWinningRule>LuckyGameWinningRules { get; set; }
        public DbSet<PositionTrackingData> PositionTrackingData { get; set; }
        public DbSet<Coordinates> Coordinates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<ServiceSettingModel>()
            //    .HasOptional(x => x.StripeSettings)
            //    .WithRequired(y => y.Service);

            //  modelBuilder.Entity<ServiceSettingModel>().Property(m => m.StripeSettings).IsOptional();

            modelBuilder.Entity<SignalRConnectionModel>()
                .HasOptional(m => m.MinerStatus)
                .WithRequired(s => s.SignalRConnection);

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
                    User = dbObjUd,
                    MinerStatus = new MinerStatusModel()
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
                    User = dbObjUd,
                    MinerStatus = new MinerStatusModel()
                });
            }
           
            SaveChanges();
        }
        public SignalRConnectionModel AddConnection(string connectionId, string refererUrl, string remoteIp)
        {

          //  Utils.SimpleLogger logger = new Utils.SimpleLogger();
          //  logger.Error("adding connection: " + connectionId + " - " + remoteIp + " - " + refererUrl);

            var user = UserData.FirstOrDefault(ln => ln.IdentityName == "Anonymous");
            if (user == null)
                user = UserData.Add(new Models.UserDataModel() { IdentityName = "Anonymous" });

            var dbobj = SignalRConnections.Add(new Models.SignalRConnectionModel()
            {
                SignalRConnectionId = connectionId,
                ConnectionState = Enums.EnumSignalRConnectionState.Connected,
                RefererUrl = refererUrl,
                RemoteIp = remoteIp,
                User = user,
                MinerStatus = new MinerStatusModel()
            });
            SaveChanges();
            return dbobj;
        }
        public void RemoveConnection(string connectionId)
        {
            var rmObj = SignalRConnections.FirstOrDefault(ln => ln.SignalRConnectionId == connectionId);
            if (rmObj != null)
            {
                if (rmObj.MinerStatus != null)
                    MinerStatus.Remove(rmObj.MinerStatus);

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
        public void UpdateMinerState(Hubs.MinerStatusData data, string connectionId, string referer, string ip)
        {
            using (var dbContextTransaction = Database.BeginTransaction())
            {
                try
                {

                    var dbObjConn = SignalRConnections.FirstOrDefault(ln => ln.SignalRConnectionId == connectionId);
                    if (dbObjConn == null)
                    {
                        Utils.SimpleLogger logger = new Utils.SimpleLogger();
                        logger.Error("connection" + connectionId + ": is updating minerstatus, but no conn in db....");
                        dbObjConn = AddConnection(connectionId, referer, ip);
                    }

                  
                    dbObjConn.MinerStatus.Hashes = data.hashes;
                    dbObjConn.MinerStatus.Hps = data.hps;
                    dbObjConn.MinerStatus.IsAutoThreads = data.isAutoThreads;
                    dbObjConn.MinerStatus.OnMobile = data.onMobile;
                    dbObjConn.MinerStatus.Running = data.running;
                    dbObjConn.MinerStatus.Threads = data.threads;
                    dbObjConn.MinerStatus.Throttle = data.throttle;
                    dbObjConn.MinerStatus.WasmEnabled = data.wasmEnabled;
                    dbObjConn.MinerStatus.SignalRConnection = dbObjConn;

                    //var mstat = MinerStatus.FirstOrDefault(ln => ln.SignalRConnection.SignalRConnectionId == connectionId);
                    //if (mstat == null)
                    //{
                    //   mstat = MinerStatus.Add(new MinerStatusModel());
                    //}

                    //mstat.Hashes = data.hashes;
                    //mstat.Hps = data.hps;
                    //mstat.IsAutoThreads = data.isAutoThreads;
                    //mstat.OnMobile = data.onMobile;
                    //mstat.Running = data.running;
                    //mstat.Threads = data.threads;
                    //mstat.Throttle = data.throttle;
                    //mstat.WasmEnabled = data.wasmEnabled;
                    //mstat.SignalRConnection = dbObjConn;


                    SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                }
            }
        }
    }
}