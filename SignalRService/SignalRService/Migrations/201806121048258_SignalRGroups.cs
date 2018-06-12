namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SignalRGroups : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SignalRGroupsModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SignalRGroupsModelSignalRConnectionModels",
                c => new
                    {
                        SignalRGroupsModel_Id = c.Int(nullable: false),
                        SignalRConnectionModel_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SignalRGroupsModel_Id, t.SignalRConnectionModel_ID })
                .ForeignKey("dbo.SignalRGroupsModels", t => t.SignalRGroupsModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.SignalRConnectionModels", t => t.SignalRConnectionModel_ID, cascadeDelete: true)
                .Index(t => t.SignalRGroupsModel_Id)
                .Index(t => t.SignalRConnectionModel_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SignalRGroupsModelSignalRConnectionModels", "SignalRConnectionModel_ID", "dbo.SignalRConnectionModels");
            DropForeignKey("dbo.SignalRGroupsModelSignalRConnectionModels", "SignalRGroupsModel_Id", "dbo.SignalRGroupsModels");
            DropIndex("dbo.SignalRGroupsModelSignalRConnectionModels", new[] { "SignalRConnectionModel_ID" });
            DropIndex("dbo.SignalRGroupsModelSignalRConnectionModels", new[] { "SignalRGroupsModel_Id" });
            DropTable("dbo.SignalRGroupsModelSignalRConnectionModels");
            DropTable("dbo.SignalRGroupsModels");
        }
    }
}
