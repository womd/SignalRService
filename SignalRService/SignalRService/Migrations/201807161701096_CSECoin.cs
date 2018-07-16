namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CSECoin : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MinerConfigurationModels", newName: "CoinIMPMinerConfigurationModels");
            RenameColumn(table: "dbo.ServiceSettingModels", name: "MinerConfiguration_ID", newName: "CoinIMPMinerConfiguration_ID");
            RenameIndex(table: "dbo.ServiceSettingModels", name: "IX_MinerConfiguration_ID", newName: "IX_CoinIMPMinerConfiguration_ID");
            CreateTable(
                "dbo.JSECoinMinerConfigurationModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ClientId = c.String(),
                        SiteId = c.String(),
                        SubId = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        Archived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.ServiceSettingModels", "JSECoinMinerConfiguration_ID", c => c.Int());
            CreateIndex("dbo.ServiceSettingModels", "JSECoinMinerConfiguration_ID");
            AddForeignKey("dbo.ServiceSettingModels", "JSECoinMinerConfiguration_ID", "dbo.JSECoinMinerConfigurationModels", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceSettingModels", "JSECoinMinerConfiguration_ID", "dbo.JSECoinMinerConfigurationModels");
            DropIndex("dbo.ServiceSettingModels", new[] { "JSECoinMinerConfiguration_ID" });
            DropColumn("dbo.ServiceSettingModels", "JSECoinMinerConfiguration_ID");
            DropTable("dbo.JSECoinMinerConfigurationModels");
            RenameIndex(table: "dbo.ServiceSettingModels", name: "IX_CoinIMPMinerConfiguration_ID", newName: "IX_MinerConfiguration_ID");
            RenameColumn(table: "dbo.ServiceSettingModels", name: "CoinIMPMinerConfiguration_ID", newName: "MinerConfiguration_ID");
            RenameTable(name: "dbo.CoinIMPMinerConfigurationModels", newName: "MinerConfigurationModels");
        }
    }
}
