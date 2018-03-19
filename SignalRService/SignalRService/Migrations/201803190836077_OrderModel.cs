namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderIdentifier = c.String(),
                        OrderState = c.Int(nullable: false),
                        CustomerUser_ID = c.Int(),
                        StoreUser_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserDataModels", t => t.CustomerUser_ID)
                .ForeignKey("dbo.UserDataModels", t => t.StoreUser_ID)
                .Index(t => t.CustomerUser_ID)
                .Index(t => t.StoreUser_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderModels", "StoreUser_ID", "dbo.UserDataModels");
            DropForeignKey("dbo.OrderModels", "CustomerUser_ID", "dbo.UserDataModels");
            DropIndex("dbo.OrderModels", new[] { "StoreUser_ID" });
            DropIndex("dbo.OrderModels", new[] { "CustomerUser_ID" });
            DropTable("dbo.OrderModels");
        }
    }
}
