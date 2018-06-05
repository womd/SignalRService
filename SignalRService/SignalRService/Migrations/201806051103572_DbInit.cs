namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coordinates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Altitude = c.Double(nullable: false),
                        Accuracy = c.Double(nullable: false),
                        AltitudeAccuracy = c.Double(nullable: false),
                        Heading = c.Double(nullable: false),
                        Speed = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GeneralSettingsModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        GeneralSetting = c.Int(nullable: false),
                        Value = c.String(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.GeneralSetting, unique: true);
            
            CreateTable(
                "dbo.LocalizationModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Culture = c.String(maxLength: 10),
                        Key = c.String(maxLength: 40),
                        Value = c.String(),
                        LastModDate = c.DateTime(nullable: false),
                        ModUser = c.String(),
                        WasHit = c.Boolean(nullable: false),
                        TranslationStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => new { t.Culture, t.Key }, unique: true, name: "IX_Localization_Culture_Key");
            
            CreateTable(
                "dbo.LuckyGameSettingsModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MoneyAvailable = c.Double(nullable: false),
                        ServiceSettings_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ServiceSettingModels", t => t.ServiceSettings_ID)
                .Index(t => t.ServiceSettings_ID);
            
            CreateTable(
                "dbo.ServiceSettingModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(),
                        ServiceUrl = c.String(maxLength: 16),
                        ServiceType = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                        MinerConfiguration_ID = c.Int(),
                        Owner_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MinerConfigurationModels", t => t.MinerConfiguration_ID)
                .ForeignKey("dbo.UserDataModels", t => t.Owner_ID)
                .Index(t => t.ServiceUrl, unique: true, name: "ServiceUrl_Index")
                .Index(t => t.MinerConfiguration_ID)
                .Index(t => t.Owner_ID);
            
            CreateTable(
                "dbo.MinerConfigurationModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ScriptUrl = c.String(),
                        ClientId = c.String(),
                        Throttle = c.Single(nullable: false),
                        StartDelayMs = c.Int(nullable: false),
                        ReportStatusIntervalMs = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserDataModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IdentityName = c.String(),
                        TotalMoney = c.Double(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductImportConfigurationModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Source = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Owner_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserDataModels", t => t.Owner_ID, cascadeDelete: true)
                .Index(t => t.Owner_ID);
            
            CreateTable(
                "dbo.ProductModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductIdentifier = c.String(maxLength: 450),
                        PartNo = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        ImageUrl = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SrcIdentifier = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                        Owner_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserDataModels", t => t.Owner_ID)
                .Index(t => t.ProductIdentifier, unique: true)
                .Index(t => t.Owner_ID);
            
            CreateTable(
                "dbo.ProductImportModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerIdString = c.String(),
                        SrcId = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        GoogleProductCategory = c.String(),
                        ProductType = c.String(),
                        Link = c.String(),
                        ImageLink = c.String(),
                        PriceString = c.String(),
                        Condition = c.String(),
                        Availabiliby = c.String(),
                        Gtin = c.String(),
                        Mpn = c.String(),
                        Brand = c.String(),
                        CustomLabel0 = c.String(),
                        CustomLabel1 = c.String(),
                        CustomLabel2 = c.String(),
                        CustomLabel3 = c.String(),
                        CustomLabel4 = c.String(),
                        gGuid = c.String(),
                        IdentifierExists = c.Boolean(nullable: false),
                        ShippingCountry = c.String(),
                        ShippingService = c.String(),
                        ShippingPrice = c.String(),
                        Owner_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserDataModels", t => t.Owner_ID, cascadeDelete: true)
                .Index(t => t.Owner_ID);
            
            CreateTable(
                "dbo.SignalRConnectionModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SignalRConnectionId = c.String(),
                        ConnectionState = c.Int(nullable: false),
                        RefererUrl = c.String(),
                        RemoteIp = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                        User_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserDataModels", t => t.User_ID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.MinerStatusModels",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Running = c.Boolean(nullable: false),
                        OnMobile = c.Boolean(nullable: false),
                        WasmEnabled = c.Boolean(nullable: false),
                        IsAutoThreads = c.Boolean(nullable: false),
                        Hps = c.Single(nullable: false),
                        Threads = c.Int(nullable: false),
                        Throttle = c.Single(nullable: false),
                        Hashes = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SignalRConnectionModels", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.StripeSettingsModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SecretKey = c.String(),
                        PublishableKey = c.String(),
                        Service_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceSettingModels", t => t.Service_ID)
                .Index(t => t.Service_ID);
            
            CreateTable(
                "dbo.LuckyGameWinningRules",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        AmountMatchingCards = c.Int(nullable: false),
                        WinFactor = c.Single(nullable: false),
                        LuckyGameSettingsModel_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.LuckyGameSettingsModels", t => t.LuckyGameSettingsModel_ID)
                .Index(t => t.LuckyGameSettingsModel_ID);
            
            CreateTable(
                "dbo.OrderItemModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PartNo = c.String(),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                        Order_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OrderModels", t => t.Order_ID)
                .Index(t => t.Order_ID);
            
            CreateTable(
                "dbo.OrderModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderIdentifier = c.String(),
                        OrderState = c.Int(nullable: false),
                        OrderType = c.Int(nullable: false),
                        PaymentState = c.Int(nullable: false),
                        ShippingState = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                        CustomerUser_ID = c.Int(),
                        StoreUser_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserDataModels", t => t.CustomerUser_ID)
                .ForeignKey("dbo.UserDataModels", t => t.StoreUser_ID)
                .Index(t => t.CustomerUser_ID)
                .Index(t => t.StoreUser_ID);
            
            CreateTable(
                "dbo.OrderJournalModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderState = c.Int(nullable: false),
                        CustomerUser_ID = c.Int(),
                        Order_ID = c.Int(),
                        StoreUser_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserDataModels", t => t.CustomerUser_ID)
                .ForeignKey("dbo.OrderModels", t => t.Order_ID)
                .ForeignKey("dbo.UserDataModels", t => t.StoreUser_ID)
                .Index(t => t.CustomerUser_ID)
                .Index(t => t.Order_ID)
                .Index(t => t.StoreUser_ID);
            
            CreateTable(
                "dbo.PositionTrackingDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeStamp = c.DateTime(nullable: false),
                        Coords_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coordinates", t => t.Coords_Id)
                .Index(t => t.Coords_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PositionTrackingDatas", "Coords_Id", "dbo.Coordinates");
            DropForeignKey("dbo.OrderJournalModels", "StoreUser_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.OrderJournalModels", "Order_ID", "dbo.OrderModels");
            DropForeignKey("dbo.OrderJournalModels", "CustomerUser_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.OrderModels", "StoreUser_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.OrderItemModels", "Order_ID", "dbo.OrderModels");
            DropForeignKey("dbo.OrderModels", "CustomerUser_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.LuckyGameWinningRules", "LuckyGameSettingsModel_ID", "dbo.LuckyGameSettingsModels");
            DropForeignKey("dbo.StripeSettingsModels", "Service_ID", "dbo.ServiceSettingModels");
            DropForeignKey("dbo.SignalRConnectionModels", "User_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.MinerStatusModels", "ID", "dbo.SignalRConnectionModels");
            DropForeignKey("dbo.ServiceSettingModels", "Owner_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.ProductImportModels", "Owner_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.ProductModels", "Owner_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.ProductImportConfigurationModels", "Owner_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.ServiceSettingModels", "MinerConfiguration_ID", "dbo.MinerConfigurationModels");
            DropForeignKey("dbo.LuckyGameSettingsModels", "ServiceSettings_ID", "dbo.ServiceSettingModels");
            DropIndex("dbo.PositionTrackingDatas", new[] { "Coords_Id" });
            DropIndex("dbo.OrderJournalModels", new[] { "StoreUser_ID" });
            DropIndex("dbo.OrderJournalModels", new[] { "Order_ID" });
            DropIndex("dbo.OrderJournalModels", new[] { "CustomerUser_ID" });
            DropIndex("dbo.OrderModels", new[] { "StoreUser_ID" });
            DropIndex("dbo.OrderModels", new[] { "CustomerUser_ID" });
            DropIndex("dbo.OrderItemModels", new[] { "Order_ID" });
            DropIndex("dbo.LuckyGameWinningRules", new[] { "LuckyGameSettingsModel_ID" });
            DropIndex("dbo.StripeSettingsModels", new[] { "Service_ID" });
            DropIndex("dbo.MinerStatusModels", new[] { "ID" });
            DropIndex("dbo.SignalRConnectionModels", new[] { "User_ID" });
            DropIndex("dbo.ProductImportModels", new[] { "Owner_ID" });
            DropIndex("dbo.ProductModels", new[] { "Owner_ID" });
            DropIndex("dbo.ProductModels", new[] { "ProductIdentifier" });
            DropIndex("dbo.ProductImportConfigurationModels", new[] { "Owner_ID" });
            DropIndex("dbo.ServiceSettingModels", new[] { "Owner_ID" });
            DropIndex("dbo.ServiceSettingModels", new[] { "MinerConfiguration_ID" });
            DropIndex("dbo.ServiceSettingModels", "ServiceUrl_Index");
            DropIndex("dbo.LuckyGameSettingsModels", new[] { "ServiceSettings_ID" });
            DropIndex("dbo.LocalizationModels", "IX_Localization_Culture_Key");
            DropIndex("dbo.GeneralSettingsModels", new[] { "GeneralSetting" });
            DropTable("dbo.PositionTrackingDatas");
            DropTable("dbo.OrderJournalModels");
            DropTable("dbo.OrderModels");
            DropTable("dbo.OrderItemModels");
            DropTable("dbo.LuckyGameWinningRules");
            DropTable("dbo.StripeSettingsModels");
            DropTable("dbo.MinerStatusModels");
            DropTable("dbo.SignalRConnectionModels");
            DropTable("dbo.ProductImportModels");
            DropTable("dbo.ProductModels");
            DropTable("dbo.ProductImportConfigurationModels");
            DropTable("dbo.UserDataModels");
            DropTable("dbo.MinerConfigurationModels");
            DropTable("dbo.ServiceSettingModels");
            DropTable("dbo.LuckyGameSettingsModels");
            DropTable("dbo.LocalizationModels");
            DropTable("dbo.GeneralSettingsModels");
            DropTable("dbo.Coordinates");
        }
    }
}
