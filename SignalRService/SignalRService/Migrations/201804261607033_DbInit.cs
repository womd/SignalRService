namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbInit : DbMigration
    {
        public override void Up()
        {
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
                "dbo.MinerConfigurationModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ScriptUrl = c.String(),
                        ClientId = c.String(),
                        Throttle = c.Single(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
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
                "dbo.UserDataModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IdentityName = c.String(),
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
                "dbo.ServiceSettingModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(),
                        ServiceUrl = c.String(maxLength: 16),
                        ServiceType = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                        Owner_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserDataModels", t => t.Owner_ID)
                .Index(t => t.ServiceUrl, unique: true, name: "ServiceUrl_Index")
                .Index(t => t.Owner_ID);
            
            CreateTable(
                "dbo.StripeSettingsModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SecretKey = c.String(),
                        PublishableKey = c.String(),
                        Service_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceSettingModels", t => t.Service_Id)
                .Index(t => t.Service_Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderJournalModels", "StoreUser_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.OrderJournalModels", "Order_ID", "dbo.OrderModels");
            DropForeignKey("dbo.OrderJournalModels", "CustomerUser_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.OrderModels", "StoreUser_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.OrderItemModels", "Order_ID", "dbo.OrderModels");
            DropForeignKey("dbo.OrderModels", "CustomerUser_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.SignalRConnectionModels", "User_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.StripeSettingsModels", "Service_Id", "dbo.ServiceSettingModels");
            DropForeignKey("dbo.ServiceSettingModels", "Owner_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.ProductImportModels", "Owner_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.ProductModels", "Owner_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.ProductImportConfigurationModels", "Owner_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.MinerStatusModels", "ID", "dbo.SignalRConnectionModels");
            DropIndex("dbo.OrderJournalModels", new[] { "StoreUser_ID" });
            DropIndex("dbo.OrderJournalModels", new[] { "Order_ID" });
            DropIndex("dbo.OrderJournalModels", new[] { "CustomerUser_ID" });
            DropIndex("dbo.OrderModels", new[] { "StoreUser_ID" });
            DropIndex("dbo.OrderModels", new[] { "CustomerUser_ID" });
            DropIndex("dbo.OrderItemModels", new[] { "Order_ID" });
            DropIndex("dbo.StripeSettingsModels", new[] { "Service_Id" });
            DropIndex("dbo.ServiceSettingModels", new[] { "Owner_ID" });
            DropIndex("dbo.ServiceSettingModels", "ServiceUrl_Index");
            DropIndex("dbo.ProductImportModels", new[] { "Owner_ID" });
            DropIndex("dbo.ProductModels", new[] { "Owner_ID" });
            DropIndex("dbo.ProductModels", new[] { "ProductIdentifier" });
            DropIndex("dbo.ProductImportConfigurationModels", new[] { "Owner_ID" });
            DropIndex("dbo.SignalRConnectionModels", new[] { "User_ID" });
            DropIndex("dbo.MinerStatusModels", new[] { "ID" });
            DropIndex("dbo.LocalizationModels", "IX_Localization_Culture_Key");
            DropIndex("dbo.GeneralSettingsModels", new[] { "GeneralSetting" });
            DropTable("dbo.OrderJournalModels");
            DropTable("dbo.OrderModels");
            DropTable("dbo.OrderItemModels");
            DropTable("dbo.StripeSettingsModels");
            DropTable("dbo.ServiceSettingModels");
            DropTable("dbo.ProductImportModels");
            DropTable("dbo.ProductModels");
            DropTable("dbo.ProductImportConfigurationModels");
            DropTable("dbo.UserDataModels");
            DropTable("dbo.SignalRConnectionModels");
            DropTable("dbo.MinerStatusModels");
            DropTable("dbo.MinerConfigurationModels");
            DropTable("dbo.LocalizationModels");
            DropTable("dbo.GeneralSettingsModels");
        }
    }
}
