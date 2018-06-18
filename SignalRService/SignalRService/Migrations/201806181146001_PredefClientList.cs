namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PredefClientList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PredefinedMinerClientModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScriptUrl = c.String(),
                        ClientId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PredefinedMinerClientModels");
        }
    }
}
