namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MiningRoom : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MiningRoomModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ServiceSettingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceSettingModels", t => t.ServiceSettingId, cascadeDelete: true)
                .Index(t => t.ServiceSettingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MiningRoomModels", "ServiceSettingId", "dbo.ServiceSettingModels");
            DropIndex("dbo.MiningRoomModels", new[] { "ServiceSettingId" });
            DropTable("dbo.MiningRoomModels");
        }
    }
}
