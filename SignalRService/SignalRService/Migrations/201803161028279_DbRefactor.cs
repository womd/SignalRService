namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbRefactor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MinerStatusModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SignalRConnectionID = c.Int(nullable: false),
                        Running = c.Boolean(nullable: false),
                        OnMobile = c.Boolean(nullable: false),
                        WasmEnabled = c.Boolean(nullable: false),
                        IsAutoThreads = c.Boolean(nullable: false),
                        Hps = c.Single(nullable: false),
                        Threads = c.Int(nullable: false),
                        Throttle = c.Single(nullable: false),
                        Hashes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SignalRConnectionModels", t => t.SignalRConnectionID, cascadeDelete: true)
                .Index(t => t.SignalRConnectionID);
            
            CreateTable(
                "dbo.SignalRConnectionModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SignalRConnectionId = c.String(),
                        ConnectionState = c.Int(nullable: false),
                        RefererUrl = c.String(),
                        RemoteIp = c.String(),
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
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SignalRConnectionModels", "User_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.MinerStatusModels", "SignalRConnectionID", "dbo.SignalRConnectionModels");
            DropIndex("dbo.SignalRConnectionModels", new[] { "User_ID" });
            DropIndex("dbo.MinerStatusModels", new[] { "SignalRConnectionID" });
            DropTable("dbo.UserDataModels");
            DropTable("dbo.SignalRConnectionModels");
            DropTable("dbo.MinerStatusModels");
        }
    }
}
