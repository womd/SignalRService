namespace SignalRService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderItems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItemModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PartNo = c.String(),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Order_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OrderModels", t => t.Order_ID)
                .Index(t => t.Order_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItemModels", "Order_ID", "dbo.OrderModels");
            DropIndex("dbo.OrderItemModels", new[] { "Order_ID" });
            DropTable("dbo.OrderItemModels");
        }
    }
}
