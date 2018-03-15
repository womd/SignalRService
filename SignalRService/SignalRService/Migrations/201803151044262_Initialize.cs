namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountPropertiesModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ServiceSettingModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ServiceName = c.String(),
                        ServiceUrl = c.String(maxLength: 16),
                        ServiceType = c.Int(nullable: false),
                        Owner_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AccountPropertiesModels", t => t.Owner_ID)
                .Index(t => t.ServiceUrl, unique: true, name: "ServiceUrl_Index")
                .Index(t => t.Owner_ID);
            
            CreateTable(
                "dbo.MinerConfigurationModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ScriptUrl = c.String(),
                        ClientId = c.String(),
                        Throttle = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceSettingModels", "Owner_ID", "dbo.AccountPropertiesModels");
            DropIndex("dbo.ServiceSettingModels", new[] { "Owner_ID" });
            DropIndex("dbo.ServiceSettingModels", "ServiceUrl_Index");
            DropTable("dbo.MinerConfigurationModels");
            DropTable("dbo.ServiceSettingModels");
            DropTable("dbo.AccountPropertiesModels");
        }
    }
}
