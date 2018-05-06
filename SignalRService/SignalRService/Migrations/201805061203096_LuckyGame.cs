namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LuckyGame : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LuckyGameSettingsModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MoneyAvailable = c.Double(nullable: false),
                        ServiceSettings_Id = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ServiceSettingModels", t => t.ServiceSettings_Id)
                .Index(t => t.ServiceSettings_Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LuckyGameWinningRules", "LuckyGameSettingsModel_ID", "dbo.LuckyGameSettingsModels");
            DropForeignKey("dbo.LuckyGameSettingsModels", "ServiceSettings_Id", "dbo.ServiceSettingModels");
            DropIndex("dbo.LuckyGameWinningRules", new[] { "LuckyGameSettingsModel_ID" });
            DropIndex("dbo.LuckyGameSettingsModels", new[] { "ServiceSettings_Id" });
            DropTable("dbo.LuckyGameWinningRules");
            DropTable("dbo.LuckyGameSettingsModels");
        }
    }
}
